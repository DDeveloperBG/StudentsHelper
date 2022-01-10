namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Subjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public class TeachersController : AdministrationController
    {
        private readonly ITeachersService teachersService;
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly IWebHostEnvironment hostingEnvironment;

        public TeachersController(
            ITeachersService teachersService,
            ISchoolSubjectsService schoolSubjectsService,
            IWebHostEnvironment hostingEnvironment)
        {
            this.teachersService = teachersService;
            this.schoolSubjectsService = schoolSubjectsService;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult AllToApprove()
        {
            var teachers = this.teachersService.GetAllNotConfirmed<NotDetailedTeacherViewModel>();
            return this.View(teachers);
        }

        public IActionResult Details(string teacherId)
        {
            var teacherData = this.teachersService.GetOne<TeacherDetails>(teacherId, false);

            return this.View(teacherData);
        }

        public IActionResult Download(string fileName)
        {
            if (fileName == null)
            {
                return this.Redirect("./Home/Index").WithDanger("Файла не бе намерен.");
            }

            string folderPath = Path.Combine(this.hostingEnvironment.ContentRootPath, "QualificationDocuments", fileName);

            return this.File(
                System.IO.File.ReadAllBytes(folderPath),
                ValidationConstants.ValidExteinsionsForQualificationDocumentToMime[Path.GetExtension(fileName)],
                $"qualification_document{Path.GetExtension(fileName)}",
                true);
        }

        public IActionResult SetTeacherData(string teacherId, string teacherName)
        {
            var model = new TeacherExternalData
            {
                Id = teacherId,
                ViewData = new TeacherExternalDataViewData
                {
                    ApplicationUserName = teacherName,
                    Subjects = this.schoolSubjectsService.GetAll<SchoolSubjectPickViewModel>(),
                },
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetTeacherData(TeacherExternalData input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View().WithWarning("Невалидни данни!");
            }

            await this.teachersService.AcceptTeacherAsync(input.Id, input.SubjectsId);

            return this.RedirectToAction(nameof(this.AllToApprove)).WithSuccess("Задачата бе изпълнена успешно.");
        }

        public async Task<IActionResult> Reject(string teacherId)
        {
            await this.teachersService.RejectTeacherAsync(teacherId);

            return this.RedirectToAction(nameof(this.AllToApprove)).WithSuccess("Задачата бе изпълнена успешно.");
        }
    }
}
