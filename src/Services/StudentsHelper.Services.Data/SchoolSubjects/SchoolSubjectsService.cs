namespace StudentsHelper.Services.Data.SchoolSubjects
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class SchoolSubjectsService : ISchoolSubjectsService
    {
        private readonly IDeletableEntityRepository<SchoolSubject> schoolSubjectsRepository;

        public SchoolSubjectsService(IDeletableEntityRepository<SchoolSubject> schoolSubjectsRepository)
        {
            this.schoolSubjectsRepository = schoolSubjectsRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.schoolSubjectsRepository.AllAsNoTracking().To<T>().ToList();
        }

        public IEnumerable<SchoolSubject> GetAllRaw()
        {
            return this.schoolSubjectsRepository.AllAsNoTracking().ToList();
        }

        public IEnumerable<int> GetTeacherSubjectsIds(string teacherId)
        {
            return this.schoolSubjectsRepository
                .AllAsNoTracking()
                .Where(x => x.Teachers.Any(x => x.Id == teacherId))
                .Select(x => x.Id)
                .ToList();
        }
    }
}
