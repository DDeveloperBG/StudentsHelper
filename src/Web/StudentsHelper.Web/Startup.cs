namespace StudentsHelper.Web
{
    using System;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;

    using IdentityModel;

    using Microsoft.AspNetCore.Authentication.OAuth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using StudentsHelper.Common;
    using StudentsHelper.Data;
    using StudentsHelper.Data.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Data.Repositories;
    using StudentsHelper.Data.Seeding;
    using StudentsHelper.Services.Auth;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Consulations;
    using StudentsHelper.Services.Data.LocationLoaders;
    using StudentsHelper.Services.Data.Meetings;
    using StudentsHelper.Services.Data.Paging;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Services.Payments;
    using StudentsHelper.Services.Payments.Models;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Services.VideoChat;
    using StudentsHelper.Web.Infrastructure.Middlewares;
    using StudentsHelper.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment currentEnvironment;

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment currentEnvironment)
        {
            this.configuration = configuration;
            this.currentEnvironment = currentEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>((x) => IdentityOptionsProvider.GetIdentityOptions(x, this.currentEnvironment.IsProduction()))
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddHangfire(
                config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(
                        this.configuration.GetConnectionString("DefaultConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true,
                        }));
            services.AddHangfireServer();

            services.AddAuthentication()
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = this.configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
                    facebookOptions.AccessDeniedPath = "/Home/Error";
                    facebookOptions.Fields.Add("picture");
                    facebookOptions.Events = new OAuthEvents
                    {
                        OnCreatingTicket = context =>
                        {
                            var identity = (ClaimsIdentity)context.Principal.Identity;
                            var profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").GetString();
                            identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
                            return Task.CompletedTask;
                        },
                    };
                });

            services.Configure<StripeOptions>(options =>
            {
                options.PublishableKey = this.configuration["Stripe:ApiKey"];
                options.SecretKey = this.configuration["Stripe:ApiSecret"];
                options.WebhookSecret = this.configuration["Stripe:WebhookSecret"];
                options.Domain = this.configuration["DOMAIN"];
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.Lax;
                        options.Secure = CookieSecurePolicy.Always;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDistributedMemoryCache();
            services.AddSession(opts =>
            {
                opts.Cookie.Name = ".Session";
                opts.IdleTimeout = TimeSpan.FromHours(24);
                opts.Cookie.HttpOnly = true;
                opts.Cookie.IsEssential = true;
                opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IMontlyPaymentsService, MontlyPaymentsService>();
            services.AddTransient<IMeetingsService, MeetingsService>();
            services.AddTransient<IDateTimeProvider, CustomDateTimeProvider>();
            services.AddTransient<ISchoolSubjectsService, SchoolSubjectsService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<IStudentsService, StudentsService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IPagingService, PagingService>();
            services.AddTransient<RegionsLoader>();
            services.AddTransient<TownshipsLoader>();
            services.AddTransient<PopulatedAreasLoader>();
            services.AddTransient<SchoolsLoader>();
            services.AddTransient<ITeacherRegisterer, TeacherRegisterer>();
            services.AddTransient<IStudentRegisterer, StudentRegisterer>();
            services.AddTransient<IPaymentsService, StripePaymentsService>();
            services.AddTransient<IConsulationsService, ConsulationsService>();
            services.AddTransient<IStudentsTransactionsService, StudentsTransactionsService>();
            services.AddTransient<IEmailSender>(_
                => new SendGridEmailSender(
                    this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender>(_
                => new SendGridEmailSender(
                    this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<IVideoChat>(_
                => new VideoChat(
                        this.configuration["VideoSDK:APIKeySid"],
                        this.configuration["DOMAIN"]));
            services.AddTransient<ICloudStorageService>(_
               => new CloudinaryService(
                       this.configuration["Cloudinary:CloudName"],
                       this.configuration["Cloudinary:ApiKey"],
                       this.configuration["Cloudinary:ApiSecret"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                this.SeedHangfireJobs(recurringJobManager);

                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (this.currentEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            // Custom Middlewares: Start
            app.UseUpdateUserActivityMiddleware();
            app.UseSetTeacherConnectedAccountMiddleware();

            // Custom Middlewares: End
            app.UseHangfireDashboard(
                "/hangfire",
                new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }

        private void SeedHangfireJobs(IRecurringJobManager recurringJobManager)
        {
            recurringJobManager
                .AddOrUpdate<IMontlyPaymentsService>(
                    "PayMontlySalaries",
                    x => x.PayMontlySalariesAsync(),
                    Cron.Monthly);
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
