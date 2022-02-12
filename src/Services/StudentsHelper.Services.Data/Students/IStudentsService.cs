namespace StudentsHelper.Services.Data.Students
{
    using System.Threading.Tasks;

    public interface IStudentsService
    {
        string GetId(string userId);

        T GetOne<T>(string userId);

        Task DeleteStudentAsync(string userId);
    }
}
