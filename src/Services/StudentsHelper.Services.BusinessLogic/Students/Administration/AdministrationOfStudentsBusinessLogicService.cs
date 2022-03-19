namespace StudentsHelper.Services.BusinessLogic.Students
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Paging.NewPaging;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Web.ViewModels.Administration.Students;
    using StudentsHelper.Web.ViewModels.Paging;

    public class AdministrationOfStudentsBusinessLogicService : IAdministrationOfStudentsBusinessLogicService
    {
        private readonly IStudentsService studentsService;
        private readonly IPagingService pagingService;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationOfStudentsBusinessLogicService(
            IStudentsService studentsService,
            UserManager<ApplicationUser> userManager,
            IPagingService pagingService)
        {
            this.studentsService = studentsService;
            this.userManager = userManager;
            this.pagingService = pagingService;
        }

        public PagedResult<StudentForAllTeachersListViewModel> GetAllStudentsViewModel(int page)
        {
            var studentsAsIQuerable = this
                .studentsService
                .GetAllAsNoTracking()
                .To<StudentForAllTeachersListViewModel>();

            return this.pagingService.GetPaged(studentsAsIQuerable, page, 10);
        }

        public StudentDetailsViewModel GetDetailsViewModel(string studentId)
        {
            return this
                .studentsService
                .GetOneFromStudentId<StudentDetailsViewModel>(studentId);
        }

        public StudentEditViewModel GetEditViewModel(string studentId)
        {
            return this
                .studentsService
                .GetOneFromStudentId<StudentEditViewModel>(studentId);
        }

        public async Task<(bool HasSucceeded, string Message)> EditAsync(string id, StudentDetailsViewModel studentData)
        {
            if (id != studentData.Id)
            {
                return (false, GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            var student = this.studentsService.GetOneTracked(id);

            if (student == null)
            {
                return (false, GlobalConstants.StudentMessages.StudentNotFoundMessage);
            }

            student.ApplicationUser = await this.userManager.FindByIdAsync(student.ApplicationUserId);

            if (student.ApplicationUser.Name != studentData.ApplicationUserName)
            {
                student.ApplicationUser.Name = studentData.ApplicationUserName;
            }

            if (student.ApplicationUser.Email != studentData.ApplicationUserEmail)
            {
                student.ApplicationUser.Email = studentData.ApplicationUserEmail;
            }

            if (student.ApplicationUser.PicturePath != studentData.ApplicationUserPicturePath)
            {
                student.ApplicationUser.PicturePath = studentData.ApplicationUserPicturePath;
            }

            await this.studentsService.UpdateAsync(student);

            return (true, GlobalConstants.GeneralMessages.TaskSucceededMessage);
        }
    }
}
