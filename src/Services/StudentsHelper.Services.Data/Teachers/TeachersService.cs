namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeachersService : ITeachersService
    {
        private readonly IDeletableEntityRepository<Teacher> teachersRepository;

        public TeachersService(IDeletableEntityRepository<Teacher> teachersRepository)
        {
            this.teachersRepository = teachersRepository;
        }

        public IEnumerable<T> GetAllNotRejected<T>(bool validated)
        {
            return this.teachersRepository
                .All()
                .Where(x => x.IsValidated == validated && !x.IsRejected)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllRejected<T>()
        {
            return this.teachersRepository
                .All()
                .Where(x => x.IsRejected)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetAllOfType<T>(int subjectId)
        {
            return this.teachersRepository
                .All()
                .Where(x => x.Subjects
                    .Any(x => x.Id == subjectId) && x.IsValidated)
                .To<T>()
                .ToList();
        }
    }
}
