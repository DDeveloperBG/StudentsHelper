namespace StudentsHelper.Services.Data.Students
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Models;

    public interface IStudentsService
    {
        string GetId(string userId);

        T GetOne<T>(string userId);

        T GetOneFromStudentId<T>(string studentId);

        Student GetOneTracked(string userId);

        Task DeleteStudentAsync(string userId);

        IQueryable<Student> GetAllAsNoTracking();

        Task UpdateAsync(Student teacher);
    }
}
