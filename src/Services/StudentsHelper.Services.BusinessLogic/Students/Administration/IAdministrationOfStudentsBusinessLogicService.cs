namespace StudentsHelper.Services.BusinessLogic.Students
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Administration.Students;
    using StudentsHelper.Web.ViewModels.Paging;

    public interface IAdministrationOfStudentsBusinessLogicService
    {
        public PagedResultModel<StudentForAllTeachersListViewModel> GetAllStudentsViewModel(int page);

        public StudentDetailsViewModel GetDetailsViewModel(string studentId);

        public StudentEditViewModel GetEditViewModel(string studentId);

        public Task<(bool HasSucceeded, string Message)> EditAsync(string id, StudentDetailsViewModel studentData);
    }
}
