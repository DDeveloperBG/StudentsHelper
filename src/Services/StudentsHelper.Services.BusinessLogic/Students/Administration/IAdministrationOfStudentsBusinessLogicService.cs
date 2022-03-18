namespace StudentsHelper.Services.BusinessLogic.Students
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Administration.Students;

    public interface IAdministrationOfStudentsBusinessLogicService
    {
        public IEnumerable<StudentForAllTeachersListViewModel> GetAllStudentsViewModel();

        public StudentDetailsViewModel GetDetailsViewModel(string studentId);

        public StudentEditViewModel GetEditViewModel(string studentId);

        public Task<(bool HasSucceeded, string Message)> EditAsync(string id, StudentDetailsViewModel studentData);
    }
}
