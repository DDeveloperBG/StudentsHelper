namespace StudentsHelper.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class DeletedUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var usersRepository = serviceProvider.GetRequiredService<IDeletableEntityRepository<ApplicationUser>>();

            if (usersRepository.AllAsNoTrackingWithDeleted().Any(x => x.UserName == GlobalConstants.DeletedUserUsername))
            {
                return;
            }

            await usersRepository.AddAsync(new ApplicationUser
            {
                Name = GlobalConstants.DeletedUserUsername,
                UserName = GlobalConstants.DeletedUserUsername,
                IsDeleted = true,
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
