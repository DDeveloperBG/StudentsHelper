namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Services.BusinessLogic.Students;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Students;

    public class StudentsController : AdministrationController
    {
        private readonly IAdministrationOfStudentsBusinessLogicService studentsBusinessLogicService;

        public StudentsController(
            IAdministrationOfStudentsBusinessLogicService studentsBusinessLogicService)
        {
            this.studentsBusinessLogicService = studentsBusinessLogicService;
        }

        public IActionResult AllStudents()
        {
            var students = this
                .studentsBusinessLogicService
                .GetAllStudentsViewModel();

            return this.View(students);
        }

        public IActionResult Details(string studentId)
        {
            if (studentId == null)
            {
                return this.NotFound();
            }

            var student = this.studentsBusinessLogicService.GetDetailsViewModel(studentId);
            if (student == null)
            {
                return this.NotFound();
            }

            return this.View(student);
        }

        public IActionResult Edit(string studentId)
        {
            if (studentId == null)
            {
                return this.NotFound();
            }

            var student = this.studentsBusinessLogicService.GetEditViewModel(studentId);
            if (student == null)
            {
                return this.NotFound();
            }

            return this.View(student);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, StudentDetailsViewModel studentData)
        {
            var allStudentsPageRedirect = this.RedirectToAction(nameof(this.AllStudents));

            if (!this.ModelState.IsValid)
            {
                return allStudentsPageRedirect.WithWarning(ValidationConstants.GeneralError);
            }

            var result = await this
                .studentsBusinessLogicService
                .EditAsync(id, studentData);

            if (!result.HasSucceeded)
            {
                return allStudentsPageRedirect.WithWarning(result.Message);
            }

            return allStudentsPageRedirect.WithSuccess(result.Message);
        }
    }
}
