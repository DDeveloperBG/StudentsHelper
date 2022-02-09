namespace StudentsHelper.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;

    public class AdminSeeder : ISeeder
    {
        private const string AdminName = "Admin";
        private const string AdminUsernamePath = "Admin:Username";
        private const string AdminPasswordKeyPath = "Admin:Password";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();

            var adminUsername = configuration[AdminUsernamePath];
            if (!dbContext.Users.Any(user => user.UserName == adminUsername))
            {
                var user = new ApplicationUser { Name = AdminName, UserName = adminUsername, Email = adminUsername };
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                await userManager.CreateAsync(user, configuration[AdminPasswordKeyPath]);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);

                var userId = await userManager.GetUserIdAsync(user);
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                await userManager.ConfirmEmailAsync(user, code);
            }
        }
    }
}
