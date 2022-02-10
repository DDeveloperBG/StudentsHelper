namespace StudentsHelper.Services.Data.SchoolSubjects
{
    using System.Collections.Generic;

    using StudentsHelper.Data.Models;

    public interface ISchoolSubjectsService
    {
        IEnumerable<T> GetAll<T>();

        public IEnumerable<SchoolSubject> GetAllRaw();

        public IEnumerable<int> GetTeacherSubjectsIds(string teacherId);
    }
}
