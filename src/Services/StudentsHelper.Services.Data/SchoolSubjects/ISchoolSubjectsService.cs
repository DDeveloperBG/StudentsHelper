namespace StudentsHelper.Services.Data.SchoolSubjects
{
    using System.Collections.Generic;

    public interface ISchoolSubjectsService
    {
        IEnumerable<T> GetAll<T>();
    }
}
