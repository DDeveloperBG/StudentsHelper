namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using System.Threading.Tasks;

    using StudentsHelper.Services.Data.Ratings.Models;
    using StudentsHelper.Web.ViewModels.Locations;
    using StudentsHelper.Web.ViewModels.Teachers;

    public interface ITeachersBusinessLogicService
    {
        (string ErrorMessage, TeachersOfSubjectType<TeacherWithRating> ViewModel) GetAllViewModel(
            int subjectId,
            int page,
            LocationInputModel locationInputModel,
            string sortBy,
            bool? isAscending);

        TeacherDetails GetDetailsViewModel(string teacherId, string userId, bool isUserStudent);

        Task UpdateDescription(string userId, string description);

        TeacherDescriptionViewModel GetDescriptionViewModel(string userId);

        string GetTeacherId(string userId);
    }
}
