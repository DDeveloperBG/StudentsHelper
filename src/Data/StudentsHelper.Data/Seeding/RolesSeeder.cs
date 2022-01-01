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

    public class RolesSeeder : ISeeder
    {
        private const string AdminName = "Admin";
        private const string AdminUsername = "admin@admin.bg";
        private const string AdminPasswordKey = "Admin:Password";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.TeacherRoleName);
            await SeedRoleAsync(roleManager, GlobalConstants.StudentRoleName);

            if (!dbContext.Users.Any(user => user.UserName == AdminUsername))
            {
                var user = new ApplicationUser { Name = AdminName, UserName = AdminUsername, Email = AdminUsername };
                var configuration = serviceProvider.GetService<IConfiguration>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                await userManager.CreateAsync(user, configuration[AdminPasswordKey]);
                await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
