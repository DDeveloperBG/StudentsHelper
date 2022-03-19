namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Services.BusinessLogic.Teachers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public class TeachersController : AdministrationController
    {
        private readonly IAdministrationOfTeachersBusinessLogicService teachersBusinessLogicService;

        public TeachersController(
            IAdministrationOfTeachersBusinessLogicService teachersBusinessLogicService)
        {
            this.teachersBusinessLogicService = teachersBusinessLogicService;
        }

        public IActionResult AllToApprove(int page = 1)
        {
            var teachers = this.teachersBusinessLogicService.GetAllToApproveViewModel(page);

            return this.View(teachers);
        }

        public IActionResult SetTeacherData(string teacherId)
        {
            var teacherData = this.teachersBusinessLogicService.GetSetTeacherDataViewModel(teacherId);

            return this.View(teacherData);
        }

        [HttpPost]
        public async Task<IActionResult> SetTeacherData(TeacherExternalDataInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.AllToApprove)).WithWarning(ValidationConstants.GeneralError);
            }

            var email = this.User.Identity.Name;

            await this.teachersBusinessLogicService.SetTeacherDataAsync(input, email);

            return this.RedirectToAction(nameof(this.AllToApprove)).WithSuccess(GlobalConstants.GeneralMessages.TaskSucceededMessage);
        }

        public async Task<IActionResult> Reject(string teacherId)
        {
            var email = this.User.Identity.Name;

            await this.teachersBusinessLogicService.Reject(teacherId, email);

            return this.RedirectToAction(nameof(this.AllToApprove)).WithSuccess(GlobalConstants.GeneralMessages.TaskSucceededMessage);
        }

        public IActionResult AllTeachers(int page = 1)
        {
            var teachers = this.teachersBusinessLogicService.GetAllTeachersViewModel(page);

            return this.View(teachers);
        }

        public IActionResult Details(string teacherId)
        {
            if (teacherId == null)
            {
                return this.NotFound();
            }

            var teacher = this.teachersBusinessLogicService.GetDetailsViewModel(teacherId);
            if (teacher == null)
            {
                return this.NotFound();
            }

            return this.View(teacher);
        }

        public IActionResult Edit(string teacherId)
        {
            if (teacherId == null)
            {
                return this.NotFound();
            }

            var teacher = this.teachersBusinessLogicService.GetEditViewModel(teacherId);
            if (teacher == null)
            {
                return this.NotFound();
            }

            return this.View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, TeacherDetailedViewModel teacherData)
        {
            var allTeacherPageRedirect = this.RedirectToAction(nameof(this.AllTeachers));

            var result = await this.teachersBusinessLogicService.EditAsync(id, teacherData);

            if (!result.HasSucceeded)
            {
                return allTeacherPageRedirect.WithWarning(result.Message);
            }

            return allTeacherPageRedirect.WithSuccess(result.Message);
        }
    }
}
