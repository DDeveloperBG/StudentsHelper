namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Students;

    public class StudentsController : AdministrationController
    {
        private readonly IStudentsService studentsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly UserManager<ApplicationUser> userManager;

        public StudentsController(
            IStudentsService studentsService,
            ICloudStorageService cloudStorageService,
            UserManager<ApplicationUser> userManager)
        {
            this.studentsService = studentsService;
            this.cloudStorageService = cloudStorageService;
            this.userManager = userManager;
        }

        public IActionResult AllStudents()
        {
            var students = this.studentsService.GetAllAsNoTracking().To<StudentForAllTeachersListViewModel>().ToList();

            return this.View(students);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var student = this.studentsService.GetOneFromStudentId<StudentDetailsViewModel>(id);
            if (student == null)
            {
                return this.NotFound();
            }

            return this.View(student);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var student = this.studentsService.GetOneFromStudentId<StudentEditViewModel>(id);
            if (student == null)
            {
                return this.NotFound();
            }

            return this.View(student);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, StudentDetailsViewModel studentData)
        {
            if (id != studentData.Id)
            {
                return this.NotFound();
            }

            var allStudentsPageRedirect = this.RedirectToAction(nameof(this.AllStudents));

            if (this.ModelState.IsValid)
            {
                var student = this.studentsService.GetOneTracked(id);

                if (student == null)
                {
                    return allStudentsPageRedirect.WithWarning("Учителя не бе намерен!");
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

                return allStudentsPageRedirect.WithSuccess("Успешно се изпълни!");
            }

            return allStudentsPageRedirect.WithWarning("Невалидни данни!");
        }
    }
}
