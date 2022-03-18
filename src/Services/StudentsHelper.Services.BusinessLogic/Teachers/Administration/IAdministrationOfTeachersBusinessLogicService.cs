namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public interface IAdministrationOfTeachersBusinessLogicService
    {
        IEnumerable<NotDetailedTeacherViewModel> GetAllToApproveViewModel();

        TeacherDetailsViewModel GetSetTeacherDataViewModel(string teacherId);

        Task SetTeacherDataAsync(TeacherExternalDataInputModel input, string email);

        IEnumerable<TeacherForAllTeachersListViewModel> GetAllTeachersViewModel();

        TeacherDetailedViewModel GetDetailsViewModel(string teacherId);

        TeacherDetailedViewModel GetEditViewModel(string teacherId);

        Task<(bool HasSucceeded, string Message)> EditAsync(string teacherId, TeacherDetailedViewModel teacherData);

        Task Reject(string teacherId, string email);
    }
}
