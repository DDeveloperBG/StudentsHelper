namespace StudentsHelper.Services.Data.SchoolSubjects
{
    using System.Collections.Generic;

    using StudentsHelper.Data.Models;

    public interface ISchoolSubjectsService
    {
        public IEnumerable<T> GetAll<T>();

        public IEnumerable<SchoolSubject> GetAllRaw();

        public IEnumerable<int> GetTeacherSubjectsIds(string teacherId);

        public string GetSubjectName(int subjectId);
    }
}
