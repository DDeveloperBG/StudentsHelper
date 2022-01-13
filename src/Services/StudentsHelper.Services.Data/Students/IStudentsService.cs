namespace StudentsHelper.Services.Data.Students
{
    public interface IStudentsService
    {
        string GetId(string userId);

        T GetOne<T>(string userId);
    }
}
