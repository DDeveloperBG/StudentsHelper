﻿namespace StudentsHelper.Web
{
    using System;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using StudentsHelper.Data;
    using StudentsHelper.Data.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Data.Repositories;
    using StudentsHelper.Data.Seeding;
    using StudentsHelper.Services.Auth;
    using StudentsHelper.Services.Data;
    using StudentsHelper.Services.Data.LocationLoaders;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Services.VideoChat;
    using StudentsHelper.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
                facebookOptions.AccessDeniedPath = "/Home/Error";
            });

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
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
                opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISchoolSubjectsService, SchoolSubjectsService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<RegionsLoader>();
            services.AddTransient<TownshipsLoader>();
            services.AddTransient<PopulatedAreasLoader>();
            services.AddTransient<SchoolsLoader>();
            services.AddTransient<ITeacherRegisterer, TeacherRegisterer>();
            services.AddTransient<IStudentRegisterer, StudentRegisterer>();
            services.AddTransient<IEmailSender>(_
                => new SendGridEmailSender(
                    this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<IVideoChat>(_
                => new VideoChat(
                        this.configuration["VideoSDK:APIKeySid"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
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

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
