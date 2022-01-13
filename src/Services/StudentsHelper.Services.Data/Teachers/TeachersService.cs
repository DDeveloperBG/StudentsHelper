namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeachersService : ITeachersService
    {
        private readonly IDeletableEntityRepository<Teacher> teachersRepository;
        private readonly IDeletableEntityRepository<SchoolSubject> schoolSubjectsRepository;

        public TeachersService(
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<SchoolSubject> schoolSubjectsRepository)
        {
            this.teachersRepository = teachersRepository;
            this.schoolSubjectsRepository = schoolSubjectsRepository;
        }

        public IEnumerable<T> GetAllNotConfirmed<T>()
        {
            return this.GetAllAsNoTracking()
                .Where(x => !x.IsValidated && !x.IsRejected)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllRejected<T>()
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.IsRejected)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllOfType<T>(int subjectId)
        {
            return this.GetAllOfType(subjectId)
                .To<T>()
                .ToList();
        }

        public IQueryable<Teacher> GetAllOfType(int subjectId)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.Subjects
                    .Any(x => x.Id == subjectId) && x.IsValidated && !x.IsRejected);
        }

        public T GetOne<T>(string id, bool isRejected)
        {
            return this.GetAllAsNoTracking()
                .Where(x => x.Id == id && x.IsRejected == isRejected)
                .To<T>()
                .SingleOrDefault();
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
    }
}
