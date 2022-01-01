namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;

    public interface ITeachersService
    {
        IEnumerable<T> GetAllOfType<T>(int subjectId);

        IEnumerable<T> GetAllNotApproved<T>();
    }
}
