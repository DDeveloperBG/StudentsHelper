namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Subjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Services.Messaging;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public class TeachersController : AdministrationController
    {
        private readonly ITeachersService teachersService;
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public TeachersController(
            ITeachersService teachersService,
            ISchoolSubjectsService schoolSubjectsService,
            ICloudStorageService cloudStorageService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager)
        {
            this.teachersService = teachersService;
            this.schoolSubjectsService = schoolSubjectsService;
            this.cloudStorageService = cloudStorageService;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        public IActionResult AllToApprove()
        {
            var teachers = this.teachersService.GetAllNotConfirmed<NotDetailedTeacherViewModel>()
                .Where(x =>
                {
                    GlobalVariables.TeachersConnectedAccountStatus.TryGetValue(x.ApplicationUserEmail, out bool status);
                    return status;
                });

            foreach (var teacher in teachers)
            {
                teacher.ApplicationUserPicturePath
                    = this.cloudStorageService.GetImageUri(teacher.ApplicationUserPicturePath);
            }

            return this.View(teachers);
        }

        public IActionResult SetTeacherData(string teacherId)
        {
            var teacherData = this.teachersService.GetOne<TeacherDetailsViewModel>(teacherId);
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

        public IActionResult AllTeachers()
        {
            var teachers = this.teachersService.GetAllAsNoTracking().To<TeacherForAllTeachersListViewModel>().ToList();

            return this.View(teachers);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var teacher = this.teachersService.GetOne<TeacherDetailedViewModel>(id);
            if (teacher == null)
            {
                return this.NotFound();
            }

            return this.View(teacher);
        }

        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var teacher = this.teachersService.GetOne<TeacherDetailedViewModel>(id);
            if (teacher == null)
            {
                return this.NotFound();
            }

            teacher.AllSubjects = this.schoolSubjectsService.GetAll<SchoolSubjectPickViewModel>();

            return this.View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(string id, TeacherDetailedViewModel teacherData)
        {
            if (id != teacherData.Id)
            {
                return this.NotFound();
            }

            var allTeacherPageRedirect = this.RedirectToAction(nameof(this.AllTeachers));

            if (this.ModelState.IsValid)
            {
                var teacher = this.teachersService.GetOneWithSubjectsTracked(id);

                if (teacher == null)
                {
                    return allTeacherPageRedirect.WithWarning("Учителя не бе намерен!");
                }

                teacher.ApplicationUser = await this.userManager.FindByIdAsync(teacher.ApplicationUserId);

                if (teacher.ApplicationUser.Name != teacherData.ApplicationUserName)
                {
                    teacher.ApplicationUser.Name = teacherData.ApplicationUserName;
                }

                if (teacher.ApplicationUser.Email != teacherData.ApplicationUserEmail)
                {
                    teacher.ApplicationUser.Email = teacherData.ApplicationUserEmail;
                }

                if (teacher.ApplicationUser.PicturePath != teacherData.ApplicationUserPicturePath)
                {
                    teacher.ApplicationUser.PicturePath = teacherData.ApplicationUserPicturePath;
                }

                if (teacher.QualificationDocumentPath != teacherData.QualificationDocumentPath)
                {
                    teacher.QualificationDocumentPath = teacherData.QualificationDocumentPath;
                }

                if (teacher.IsValidated != teacherData.IsValidated &&
                    teacher.IsRejected != teacherData.IsRejected &&
                    (teacherData.IsValidated || teacherData.IsRejected) &&
                    teacherData.IsValidated != teacherData.IsRejected)
                {
                    if (teacher.IsValidated && !teacherData.SelectedSubjectsIds.Any())
                    {
                        return allTeacherPageRedirect.WithWarning("Невалидни данни!");
                    }

                    teacher.IsValidated = teacherData.IsValidated;
                    teacher.IsRejected = teacherData.IsRejected;
                }

                if (teacher.IsValidated && teacherData.SelectedSubjectsIds.Any())
                {
                    var all = this.schoolSubjectsService.GetAllRaw();

                    foreach (var subject in teacherData.SelectedSubjectsIds)
                    {
                        if (!teacher.Subjects.Any(x => x.Id == subject))
                        {
                            teacher.Subjects.Add(all.Single(x => x.Id == subject));
                        }
                    }

                    foreach (var subject in teacher.Subjects)
                    {
                        if (!teacherData.SelectedSubjectsIds.Any(x => x == subject.Id))
                        {
                            teacher.Subjects.Remove(subject);
                        }
                    }
                }

                await this.teachersService.UpdateAsync(teacher);

                return allTeacherPageRedirect.WithSuccess("Успешно се изпълни!");
            }

            return allTeacherPageRedirect.WithWarning("Невалидни данни!");
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
