namespace StudentsHelper.Services.BusinessLogic.StudentFavouriteTeachers
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.StudentFavouriteTeacher;

    public interface IStudentFavouriteTeachersBusinessLogicService
    {
        Task AddOrRemoveTeacherAsync(string studentUserId, string teacherUserId);

        AllStudentFavouriteTeachersViewModel GetIndexPageViewModel(string studentUserId, int page);
    }
}
