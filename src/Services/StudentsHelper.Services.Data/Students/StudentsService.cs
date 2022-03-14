namespace StudentsHelper.Services.Data.Students
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentsService : IStudentsService
    {
        private readonly IDeletableEntityRepository<Student> studentsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public StudentsService(
            IDeletableEntityRepository<Student> studentsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.studentsRepository = studentsRepository;
            this.userManager = userManager;
        }

        public string GetId(string userId)
        {
            return this.studentsRepository.AllAsNoTracking()
               .Where(x => x.ApplicationUserId == userId)
               .Select(x => x.Id)
               .SingleOrDefault();
        }

        public T GetOne<T>(string userId)
        {
            return this.studentsRepository
               .AllAsNoTracking()
               .Where(x => x.ApplicationUserId == userId)
               .To<T>()
               .SingleOrDefault();
        }

        public T GetOneFromStudentId<T>(string studentId)
        {
            return this.studentsRepository
               .AllAsNoTracking()
               .Where(x => x.Id == studentId)
               .To<T>()
               .SingleOrDefault();
        }

        public Student GetOneTracked(string id)
        {
            return this.studentsRepository.AllAsNoTracking()
               .Where(x => x.Id == id)
               .SingleOrDefault();
        }

        public async Task DeleteStudentAsync(string userId)
        {
            await this.userManager.RemoveFromRoleAsync(new ApplicationUser { Id = userId }, GlobalConstants.StudentRoleName);

            var deletedUser = await this.userManager.FindByNameAsync(GlobalConstants.DeletedUserUsername);
            this.studentsRepository.All().Where(x => x.ApplicationUserId == userId).SingleOrDefault().ApplicationUser = deletedUser;

            await this.studentsRepository.SaveChangesAsync();
        }

        public IQueryable<Student> GetAllAsNoTracking()
        {
            return this.studentsRepository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUser.UserName != GlobalConstants.DeletedUserUsername);
        }

        public Task UpdateAsync(Student teacher)
        {
            this.studentsRepository.Update(teacher);
            return this.studentsRepository.SaveChangesAsync();
        }
    }
}
