namespace StudentsHelper.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Consulations;
    using StudentsHelper.Services.VideoChat;
    using StudentsHelper.Web.Infrastructure.Alerts;

    [Authorize]
    public class VideoChatController : Controller
    {
        private readonly IVideoChat videoChat;
        private readonly IConsulationsService consulationsService;
        private readonly UserManager<ApplicationUser> userManager;

        public VideoChatController(
            IVideoChat videoChat,
            UserManager<ApplicationUser> userManager,
            IConsulationsService consulationsService)
        {
            this.videoChat = videoChat;
            this.userManager = userManager;
            this.consulationsService = consulationsService;
        }

        public IActionResult VideoChat(string meetingId)
        {
            if (meetingId == null)
            {
                return this.Redirect("/").WithDanger("Невалидни данни!");
            }

            var userId = this.userManager.GetUserId(this.User);

            if (!this.consulationsService.IsConsultationActive(meetingId, userId))
            {
                return this.Redirect("/").WithDanger("Невалидни данни!");
            }

            return this.View();
        }

        public async Task<IActionResult> UserConfig(string meetingId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            return this.Json(this.videoChat.GetUserConfigurations(user.Name, meetingId, this.Request.Host.Value));
        }
    }
}
