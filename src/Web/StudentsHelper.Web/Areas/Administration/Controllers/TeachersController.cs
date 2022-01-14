namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Subjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Administration.Teachers;

    public class TeachersController : AdministrationController
    {
        private readonly ITeachersService teachersService;
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly ICloudStorageService cloudStorageService;

        public TeachersController(
            ITeachersService teachersService,
            ISchoolSubjectsService schoolSubjectsService,
            ICloudStorageService cloudStorageService)
        {
            this.teachersService = teachersService;
            this.schoolSubjectsService = schoolSubjectsService;
            this.cloudStorageService = cloudStorageService;
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
