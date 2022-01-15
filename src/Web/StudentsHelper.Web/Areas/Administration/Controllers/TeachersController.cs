namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Common;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Subjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public class TeachersController : AdministrationController
    {
        private readonly ITeachersService teachersService;
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly IEmailSender emailSender;

        public TeachersController(
            ITeachersService teachersService,
            ISchoolSubjectsService schoolSubjectsService,
            ICloudStorageService cloudStorageService,
            IEmailSender emailSender)
        {
            this.teachersService = teachersService;
            this.schoolSubjectsService = schoolSubjectsService;
            this.cloudStorageService = cloudStorageService;
            this.emailSender = emailSender;
        }

        public IActionResult AllToApprove()
        {
            var teachers = this.teachersService.GetAllNotConfirmed<NotDetailedTeacherViewModel>();
            foreach (var teacher in teachers)
            {
                teacher.ApplicationUserPicturePath
                    = this.cloudStorageService.GetImageUri(teacher.ApplicationUserPicturePath);
            }

            return this.View(teachers);
        }

        public IActionResult SetTeacherData(string teacherId)
        {
            var teacherData = this.teachersService.GetOne<TeacherDetailsViewModel>(teacherId, false);
            teacherData.Subjects = this.schoolSubjectsService.GetAll<SchoolSubjectPickViewModel>();
            teacherData.ApplicationUserPicturePath = this.cloudStorageService.GetImageUri(teacherData.ApplicationUserPicturePath);
            teacherData.QualificationDocumentPath = this.cloudStorageService.GetImageUri(teacherData.QualificationDocumentPath);

            return this.View(teacherData);
        }

        [HttpPost]
        public async Task<IActionResult> SetTeacherData(TeacherExternalDataInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.AllToApprove)).WithWarning("Невалидни данни!");
            }

            var email = this.User.Identity.Name;
            var message = "<h1 style=\"color: #6BB843\">Профила ви бе потвърден. Вече може да водите уроци.</h1>";
            await this.SendEmailResponce(email, message);
            await this.teachersService.AcceptTeacherAsync(input.Id, input.SubjectsId);

            return this.RedirectToAction(nameof(this.AllToApprove)).WithSuccess("Задачата бе изпълнена успешно.");
        }

        public async Task<IActionResult> Reject(string teacherId)
        {
            var email = this.User.Identity.Name;
            var message = "<h1 style=\"color: #E12735\">Профила ви бе отхвърлен. Не може да водите уроци.</h1>" +
                "<h2>Пробвайте да се регистрирате пак и качете по-добра снимка на документа ви за квалификация.</h2>";
            await this.SendEmailResponce(email, message);
            await this.teachersService.RejectTeacherAsync(teacherId);

            return this.RedirectToAction(nameof(this.AllToApprove)).WithSuccess("Задачата бе изпълнена успешно.");
        }

        private Task SendEmailResponce(string to, string message)
        {
            return this.emailSender.SendEmailAsync(
                GlobalConstants.ContactEmail,
                GlobalConstants.SystemName,
                to,
                "Профила ви бе:",
                message);
        }
    }
}
