namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface ITeachersService
    {
        IEnumerable<T> GetAllOfType<T>(int subjectId);

        IQueryable<Teacher> GetAllOfType(int subjectId);

        IEnumerable<T> GetAllRejected<T>();

        IEnumerable<T> GetAllNotConfirmed<T>();

        string GetId(string userId);

        T GetOne<T>(string id, bool isRejected);

        Task RejectTeacherAsync(string id);

        Task AcceptTeacherAsync(string id, int[] subjects);

        IQueryable<Teacher> GetAllAsNoTracking();

        IQueryable<Teacher> GetAll();
    }
}
