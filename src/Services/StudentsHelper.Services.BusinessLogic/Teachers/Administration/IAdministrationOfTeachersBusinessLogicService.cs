﻿namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Administration.Teachers;
    using StudentsHelper.Web.ViewModels.Paging;

    public interface IAdministrationOfTeachersBusinessLogicService
    {
        PagedResultModel<NotDetailedTeacherViewModel> GetAllToApproveViewModel(int page);

        TeacherDetailsViewModel GetSetTeacherDataViewModel(string teacherId);

        Task SetTeacherDataAsync(TeacherExternalDataInputModel input, string email);

        PagedResultModel<TeacherForAllTeachersListViewModel> GetAllTeachersViewModel(int page);

        TeacherDetailedViewModel GetDetailsViewModel(string teacherId);

        TeacherDetailedViewModel GetEditViewModel(string teacherId);

        Task<(bool HasSucceeded, string Message)> EditAsync(string teacherId, TeacherDetailedViewModel teacherData);

        Task Reject(string teacherId, string email);
    }
}
