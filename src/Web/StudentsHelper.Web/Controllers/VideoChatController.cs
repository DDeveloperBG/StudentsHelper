namespace StudentsHelper.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Consulations;
    using StudentsHelper.Services.Data.Meetings;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Services.VideoChat;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Meetings;

    [Authorize]
    public class VideoChatController : BaseController
    {
        private readonly IVideoChat videoChat;
        private readonly IConsulationsService consulationsService;
        private readonly IMeetingsService meetingsService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public VideoChatController(
            IVideoChat videoChat,
            UserManager<ApplicationUser> userManager,
            IConsulationsService consulationsService,
            IMeetingsService meetingsService,
            IDateTimeProvider dateTimeProvider,
            IStudentsTransactionsService studentsTransactionsService)
        {
            this.videoChat = videoChat;
            this.userManager = userManager;
            this.consulationsService = consulationsService;
            this.meetingsService = meetingsService;
            this.dateTimeProvider = dateTimeProvider;
            this.studentsTransactionsService = studentsTransactionsService;
        }

        public async Task<IActionResult> VideoChatAsync(string meetingId)
        {
            if (meetingId == null)
            {
                return this.Redirect("/").WithDanger(ValidationConstants.GeneralError);
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!this.consulationsService.IsConsultationActive(meetingId, userId, this.dateTimeProvider.GetUtcNow()))
            {
                return this.Redirect("/").WithDanger(ValidationConstants.GeneralError);
            }

            // Important!
            await this.UpdatePartiacipantStatus(meetingId);

            if (!this.StudentHasEnoughMoneyToContinue(meetingId))
            {
                return this.Redirect("/").WithInfo(ValidationConstants.MeetingEndedMessage);
            }

            return this.View();
        }

        public async Task<IActionResult> UserConfig(string meetingId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            return this.Json(this.videoChat.GetUserConfigurations(user.Name, meetingId));
        }

        public async Task<IActionResult> UpdatePartiacipantStatus(string meetingId)
        {
            // Important!
            await this.UpdateParticipantStatusAsync(meetingId);

            var userId = this.userManager.GetUserId(this.User);
            if (!this.consulationsService.IsConsultationActive(meetingId, userId, this.dateTimeProvider.GetUtcNow()))
            {
                return this.Redirect("/").WithInfo(ValidationConstants.MeetingEndedMessage);
            }

            if (!this.StudentHasEnoughMoneyToContinue(meetingId))
            {
                return this.BadRequest();
            }

            return this.Ok();
        }

        private Task UpdateParticipantStatusAsync(string meetingId)
        {
            string role;

            if (this.User.IsInRole(GlobalConstants.StudentRoleName))
            {
                role = GlobalConstants.StudentRoleName;
            }
            else if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                role = GlobalConstants.TeacherRoleName;
            }
            else
            {
                throw new System.Exception(ValidationConstants.IncorectUserRole);
            }

            var utcNow = this.dateTimeProvider.GetUtcNow();

            return this.meetingsService
                .UpdateParticipantActivityAndIncreaseDurationAsync(
                    role,
                    meetingId,
                    utcNow,
                    () => this.studentsTransactionsService.ChargeStudentAsync(meetingId, utcNow));
        }

        private bool StudentHasEnoughMoneyToContinue(string meetingId)
        {
            var neededValues = this.meetingsService.GetMeetingData<IsStudentBalanceEnoughForMoreTimeNeededDataModel>(meetingId);
            var studentBalance = this.studentsTransactionsService.GetStudentBalance(neededValues.StudentId);

            return neededValues.NextPrice <= studentBalance;
        }
    }
}
