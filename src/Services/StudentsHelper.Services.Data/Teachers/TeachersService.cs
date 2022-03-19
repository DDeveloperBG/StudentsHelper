namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeachersService : ITeachersService
    {
        private readonly IDeletableEntityRepository<Teacher> teachersRepository;
        private readonly IDeletableEntityRepository<SchoolSubject> schoolSubjectsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public TeachersService(
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<SchoolSubject> schoolSubjectsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.teachersRepository = teachersRepository;
            this.schoolSubjectsRepository = schoolSubjectsRepository;
            this.userManager = userManager;
        }

        public IQueryable<T> GetAllNotConfirmed<T>()
        {
            return this.GetAllAsNoTracking()
                .Where(x => !x.IsValidated && !x.IsRejected)
                .To<T>();
        }

        public IEnumerable<T> GetAllRejected<T>()
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.IsRejected)
                .To<T>()
                .ToList();
        }

        public IQueryable<Teacher> GetAllOfType(int subjectId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.Subjects
                    .Any(x => x.Id == subjectId) && x.IsValidated && !x.IsRejected && x.HourWage != null);
        }

        public IQueryable<Teacher> GetAllOfType(int subjectId, IQueryable<Teacher> teachers)
        {
            return teachers
                .Where(x => x.Subjects
                    .Any(x => x.Id == subjectId) && x.IsValidated && !x.IsRejected && x.HourWage != null);
        }

        public IEnumerable<T> GetAllValidatedMappedAndTracked<T>()
        {
            return this.GetAll()
                .Where(x => x.IsValidated && !x.IsRejected)
                .To<T>();
        }

        public string GetId(string userId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.Id)
                .SingleOrDefault();
        }

        public T GetOne<T>(string id)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .SingleOrDefault();
        }

        public Teacher GetOneWithSubjectsTracked(string id)
        {
            return this.GetAll()
                .Where(x => x.Id == id)
                .Include(g => g.Subjects)
                .Single();
        }

        public Task RejectTeacherAsync(string id)
        {
            var teacher = this.GetAllAsNoTracking()
                .Where(x => x.Id == id)
                .SingleOrDefault();

            teacher.IsRejected = true;
            teacher.IsValidated = true;

            return this.teachersRepository.SaveChangesAsync();
        }

        public Task AcceptTeacherAsync(string id, int[] subjects)
        {
            var teacher = this.GetAll()
              .Where(x => x.Id == id)
              .SingleOrDefault();

            teacher.IsRejected = false;
            teacher.IsValidated = true;

            foreach (var subjectId in subjects)
            {
                var subject = this.schoolSubjectsRepository.All().Where(x => x.Id == subjectId).SingleOrDefault();

                teacher.Subjects.Add(subject);
            }

            return this.teachersRepository.SaveChangesAsync();
        }

        public IQueryable<Teacher> GetAllAsNoTracking()
        {
            return this.teachersRepository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUser.UserName != GlobalConstants.DeletedUserUsername);
        }

        public IQueryable<Teacher> GetAll()
        {
            return this.teachersRepository
                .All()
                .Where(x => x.ApplicationUser.UserName != GlobalConstants.DeletedUserUsername);
        }

        public decimal? GetHourWage(string teacherId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.Id == teacherId)
                .Select(x => x.HourWage)
                .SingleOrDefault();
        }

        public Task ChangeTeacherHourWageAsync(string teacherId, decimal teacherWage)
        {
            var teacher = this.GetAll()
                .Where(x => x.Id == teacherId)
                .SingleOrDefault();

            teacher.HourWage = teacherWage;
            return this.teachersRepository.SaveChangesAsync();
        }

        public string GetExpressConnectedAccountId(string teacherId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.Id == teacherId)
                .Select(x => x.ExpressConnectedAccountId)
                .Single();
        }

        public bool IsTeacherConfirmed(string userId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.IsValidated)
                .Single();
        }

        public async Task DeleteTeacherAsync(string userId)
        {
            await this.userManager.RemoveFromRoleAsync(new ApplicationUser { Id = userId }, GlobalConstants.TeacherRoleName);

            var deletedUser = await this.userManager.FindByNameAsync(GlobalConstants.DeletedUserUsername);
            this.teachersRepository.All().Where(x => x.ApplicationUserId == userId).SingleOrDefault().ApplicationUser = deletedUser;

            await this.teachersRepository.SaveChangesAsync();
        }

        public Task UpdateAsync(Teacher teacher)
        {
            this.teachersRepository.Update(teacher);
            return this.teachersRepository.SaveChangesAsync();
        }
    }
}
