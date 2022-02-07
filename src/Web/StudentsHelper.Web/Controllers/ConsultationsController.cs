namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Hangfire;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Consulations;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Consultations;

    public class ConsultationsController : Controller
    {
        private readonly IConsulationsService consulationsService;
        private readonly IStudentsService studentsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITeachersService teachersService;
        private readonly IStudentsTransactionsService studentsTransactionsService;
        private readonly IDateTimeProvider dateTimeProvider;

        public ConsultationsController(
            IConsulationsService consulationsService,
            UserManager<ApplicationUser> userManager,
            IStudentsService studentsService,
            ITeachersService teachersService,
            IStudentsTransactionsService studentsTransactionsService,
            IDateTimeProvider dateTimeProvider)
        {
            this.consulationsService = consulationsService;
            this.userManager = userManager;
            this.studentsService = studentsService;
            this.teachersService = teachersService;
            this.studentsTransactionsService = studentsTransactionsService;
            this.dateTimeProvider = dateTimeProvider;
        }

        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> BookConsultation(int subjectId, string teacherId, string returnUrl = null)
        {
            var hourWage = this.teachersService.GetHourWage(teacherId);

            var studentUser = await this.userManager.GetUserAsync(this.User);
            if (studentUser == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            string studentId = this.studentsService.GetId(studentUser.Id);
            var studentBalance = this.studentsTransactionsService.GetStudentBalance(studentId);

            if (returnUrl != null)
            {
                this.HttpContext.Session.Set("returnUrl", Encoding.UTF8.GetBytes(returnUrl));
            }

            if (hourWage == null || studentBalance == 0)
            {
                return this.Redirect("/Identity/Balance").WithWarning("Нямате достатъчно пари.");
            }

            var viewModel = new BookConsultationInputModel
            {
                TeacherId = teacherId,
                StartTime = this.dateTimeProvider.GetUtcNow(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> BookConsultation(BookConsultationInputModel inputModel)
        {
            this.HttpContext.Session.TryGetValue("returnUrl", out byte[] outputBytesForReturnUrl);
            var returnUrl = Encoding.UTF8.GetString(outputBytesForReturnUrl);
            var responce = this.Redirect(returnUrl ?? "~/");

            var min = new TimeSpan(0, 10, 0);
            var max = new TimeSpan(5, 0, 0);

            if (!this.ModelState.IsValid || !(inputModel.Duration >= min && inputModel.Duration <= max))
            {
                this.HttpContext.Session.Remove("returnUrl");
                return responce.WithDanger("Невалидни данни");
            }

            if ((this.dateTimeProvider.GetUtcNow() - inputModel.StartTime).TotalMilliseconds > 0)
            {
                this.HttpContext.Session.Remove("returnUrl");
                return responce.WithDanger("Невалидни данни");
            }

            var endTime = inputModel.StartTime + inputModel.Duration;
            var hourWage = this.teachersService.GetHourWage(inputModel.TeacherId);

            if (hourWage == null)
            {
                this.HttpContext.Session.Remove("returnUrl");
                return responce.WithDanger("Невалидни данни");
            }

            var studentUser = await this.userManager.GetUserAsync(this.User);
            if (studentUser == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var studentId = this.studentsService.GetId(studentUser.Id);

            var studentBalance = this.studentsTransactionsService.GetStudentBalance(studentId);
            var minuteWage = hourWage.Value / 60;
            var totalPrice = (int)inputModel.Duration.TotalMinutes * minuteWage;
            if (studentBalance - totalPrice < 0)
            {
                return this.Redirect("/Identity/Balance").WithWarning("Нямате достатъчно пари.");
            }

            this.HttpContext.Session.TryGetValue("subjectId", out byte[] outputBytesForSubject);

            if (outputBytesForSubject == null)
            {
                this.HttpContext.Session.Remove("returnUrl");
                return responce.WithDanger("Невалидни данни");
            }

            int subjectId = BitConverter.ToInt32(outputBytesForSubject);

            var consultation = await this.consulationsService.AddConsultationAsync(inputModel.StartTime, endTime, hourWage.Value, inputModel.Reason, subjectId, studentId, inputModel.TeacherId);

            // Important !!!
            var jobId = BackgroundJob.Schedule(
                () => this.studentsTransactionsService.ChargeStudentAsync(consultation.MeetingId, this.dateTimeProvider.GetUtcNow()),
                consultation.Duration);

            this.HttpContext.Session.Remove("returnUrl");
            return responce.WithSuccess("Успешно резервирахте консултация.");
        }

        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> StudentConsultations()
        {
            var studentUser = await this.userManager.GetUserAsync(this.User);
            if (studentUser == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var studentId = this.studentsService.GetId(studentUser.Id);
            var viewModel = this.consulationsService.GetStudentConsultations<StudentConsultationViewModel>(studentId, this.dateTimeProvider.GetUtcNow())
                .OrderBy(x => x.ConsultationDetails.StartTime);

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.TeacherRoleName)]
        public async Task<IActionResult> TeacherConsultations()
        {
            var teacherUser = await this.userManager.GetUserAsync(this.User);
            if (teacherUser == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var teacherId = this.teachersService.GetId(teacherUser.Id);
            var viewModel = this.consulationsService.GetTeacherConsultations<TeacherConsultationsViewModel>(teacherId, this.dateTimeProvider.GetUtcNow())
                .OrderBy(x => x.ConsultationDetails.StartTime);

            return this.View(viewModel);
        }
    }
}
