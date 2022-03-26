namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface ITeachersService
    {
        IQueryable<Teacher> GetAllOfType(int subjectId);

        IQueryable<Teacher> GetAllOfType(int subjectId, IQueryable<Teacher> teachers);

        IEnumerable<T> GetAllRejected<T>();

        IQueryable<T> GetAllNotConfirmed<T>();

        string GetId(string userId);

        Teacher GetOneWithSubjectsTracked(string id);

        T GetOne<T>(string id);

        Task RejectTeacherAsync(string id);

        Task AcceptTeacherAsync(string id, int[] subjects);

        IQueryable<Teacher> GetAllAsNoTracking();

        IQueryable<Teacher> GetAll();

        IEnumerable<T> GetAllValidatedMappedAndTracked<T>();

        decimal? GetHourWage(string teacherId);

        Task ChangeTeacherHourWageAsync(string teacherId, decimal teacherWage);

        string GetExpressConnectedAccountId(string teacherId);

        bool IsTeacherConfirmed(string userId);

        Task DeleteTeacherAsync(string userId);

        Task UpdateAsync(Teacher teacher);

        Task UpdateDescription(string userId, string description);
    }
}
