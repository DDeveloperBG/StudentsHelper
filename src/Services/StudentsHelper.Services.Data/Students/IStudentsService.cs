using System.Threading.Tasks;

namespace StudentsHelper.Services.Data.Students
{
    public interface IStudentsService
    {
        string GetId(string userId);

        T GetOne<T>(string userId);

        Task DeleteStudentAsync(string userId);
    }
}
