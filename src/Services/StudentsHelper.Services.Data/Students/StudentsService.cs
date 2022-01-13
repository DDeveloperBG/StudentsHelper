namespace StudentsHelper.Services.Data.Students
{
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentsService : IStudentsService
    {
        private readonly IDeletableEntityRepository<Student> studentsRepository;

        public StudentsService(IDeletableEntityRepository<Student> studentsRepository)
        {
            this.studentsRepository = studentsRepository;
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
            return this.studentsRepository.AllAsNoTracking()
               .Where(x => x.ApplicationUserId == userId)
               .To<T>()
               .SingleOrDefault();
        }
    }
}
