namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.BusinessLogic.Consultations;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Consultations;

    [Authorize]
    public class ConsultationsController : BaseController
    {
        private readonly IConsultationsBusinessLogicService consultationsBusinessLogicService;

        public ConsultationsController(
            UserManager<ApplicationUser> userManager,
            IConsultationsBusinessLogicService consultationsBusinessLogicService)
            : base(userManager)
        {
            this.consultationsBusinessLogicService = consultationsBusinessLogicService;
        }

        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> BookConsultation(string teacherId, string returnUrl = null)
        {
            var studentUser = await this.GetCurrentUserDataAsync();
            if (studentUser == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            if (returnUrl != null)
            {
                this.HttpContext.Session.Set(GlobalConstants.ReturnUrlSessionValueKey, Encoding.UTF8.GetBytes(returnUrl));
            }

            var minDurationInMinutes = (int)ValidationConstants.Consultation.MinDuration.TotalMinutes;
            bool hasEnoughMoney = this
                .consultationsBusinessLogicService
                .DoesStudentHasEnoughMoney(
                    teacherId,
                    studentUser.Id,
                    minDurationInMinutes);

            if (!hasEnoughMoney)
            {
                return this.Redirect("/Identity/Balance")
                    .WithWarning(GlobalConstants.PaymentMessages.InsufficientBalanceMessage);
            }

            var viewModel = this
                .consultationsBusinessLogicService
                .GetBookConsultationViewModel(teacherId);

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> BookConsultation(BookConsultationInputModel inputModel)
        {
            // Get returnUrl from Session
            this.HttpContext.Session.TryGetValue(GlobalConstants.ReturnUrlSessionValueKey, out byte[] outputBytesForReturnUrl);
            this.HttpContext.Session.Remove(GlobalConstants.ReturnUrlSessionValueKey);
            var returnUrl = Encoding.UTF8.GetString(outputBytesForReturnUrl);
            IActionResult responce = this.Redirect(returnUrl ?? "~/");

            // Get current user
            var user = await this.GetCurrentUserDataAsync();
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            // Get subjectId from Session
            this.HttpContext.Session.TryGetValue(GlobalConstants.SubjectIdSessionValueKey, out byte[] outputBytesForSubject);

            if (outputBytesForSubject == null)
            {
                this.HttpContext.Session.Remove(GlobalConstants.ReturnUrlSessionValueKey);
                return responce.WithDanger(ValidationConstants.GeneralError);
            }

            int subjectId = BitConverter.ToInt32(outputBytesForSubject);

            // Book consultation
            var methodError = await this
                .consultationsBusinessLogicService
                .BookConsultation(
                    inputModel,
                    user.Id,
                    subjectId);

            // Update action responce
            if (methodError != null)
            {
                if (methodError == GlobalConstants.PaymentMessages.InsufficientBalanceMessage)
                {
                    responce = this.Redirect("/Identity/Balance");
                }

                responce = responce.WithWarning(methodError);
            }
            else
            {
                responce = responce.WithSuccess(GlobalConstants.ConsultationMessages.SuccessfulConsultationReservation);
            }

            return responce;
        }

        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> StudentConsultations()
        {
            var studentUser = await this.GetCurrentUserDataAsync();
            if (studentUser == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            var viewModel = this
                .consultationsBusinessLogicService
                .GetStudentConsultationsViewModel(studentUser.Id);

            return this.View(viewModel);
        }

        [Authorize(Roles = GlobalConstants.TeacherRoleName)]
        public async Task<IActionResult> TeacherConsultations()
        {
            var teacherUser = await this.GetCurrentUserDataAsync();
            if (teacherUser == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            var viewModel = this
                .consultationsBusinessLogicService
                .GetTeacherConsultationsViewModel(teacherUser.Id);

            return this.View(viewModel);
        }

        public IActionResult Calendar()
        {
            return this.View();
        }

        public async Task<IActionResult> GetCalendarConsultationsAsync()
        {
            var user = await this.GetCurrentUserDataAsync();
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            bool isTeacher = this.User.IsInRole(GlobalConstants.TeacherRoleName);
            bool isStudent = this.User.IsInRole(GlobalConstants.StudentRoleName);

            var consultations = this
                .consultationsBusinessLogicService
                .GetCalendarConsultationsAsync(
                    user.Id,
                    isTeacher,
                    isStudent);

            return this.Json(consultations);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ChangeConsultationStartTimeAsync([FromBody] UpdateConsultationInputModel input)
        {
            bool hasSucceeded = await this
                .consultationsBusinessLogicService
                .ChangeConsultationStartTimeAsync(input);

            if (!hasSucceeded)
            {
                return this.BadRequest();
            }

            return this.Json(new { success = true });
        }
    }
}
