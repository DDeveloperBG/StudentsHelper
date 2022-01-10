namespace StudentsHelper.Services.Data.Teachers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITeachersService
    {
        IEnumerable<T> GetAllOfType<T>(int subjectId);

        IEnumerable<T> GetAllRejected<T>();

        IEnumerable<T> GetAllNotConfirmed<T>();

        T GetOne<T>(string id, bool isRejected);

        Task RejectTeacherAsync(string id);

        Task AcceptTeacherAsync(string id, int[] subjects);
    }
}
