namespace StudentsHelper.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class DeletedUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var usersRepository = serviceProvider.GetRequiredService<IRepository<ApplicationUser>>();

            if (usersRepository.AllAsNoTracking().Any(x => x.UserName == GlobalConstants.DeletedUserUsername))
            {
                return;
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser
            {
                Name = GlobalConstants.DeletedUserUsername,
                UserName = GlobalConstants.DeletedUserUsername,
                Email = GlobalConstants.DeletedUserUsername,
            };

            var configuration = serviceProvider.GetService<IConfiguration>();

            await userManager.CreateAsync(user, configuration["DeletedUser:Password"]);
        }
    }
}
