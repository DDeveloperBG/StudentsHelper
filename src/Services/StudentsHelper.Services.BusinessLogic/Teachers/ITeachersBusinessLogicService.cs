namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using StudentsHelper.Services.Data.Ratings.Models;
    using StudentsHelper.Web.ViewModels.Locations;
    using StudentsHelper.Web.ViewModels.Teachers;

    public interface ITeachersBusinessLogicService
    {
        (string ErrorMessage, TeachersOfSubjectType<TeacherWithRating> ViewModel) GetAllViewModel(int subjectId, int page, LocationInputModel locationInputModel);

        TeacherDetails GetDetailsViewModel(string teacherId, string userId, bool isUserStudent);
    }
}
