namespace StudentsHelper.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.VideoChat;
    using StudentsHelper.Web.Infrastructure.Alerts;

    [Authorize]
    public class VideoChatController : Controller
    {
        private readonly IVideoChat videoChat;
        private readonly IRepository<Meeting> meetingsRepository;

        public VideoChatController(IVideoChat videoChat, IRepository<Meeting> meetingsRepository)
        {
            this.videoChat = videoChat;
            this.meetingsRepository = meetingsRepository;
        }

        public IActionResult VideoChat(string meetingId)
        {
            if (meetingId == null)
            {
                return this.Redirect("/").WithDanger("Невалидни данни!");
            }

            var id = this.meetingsRepository.All().Where(x => x.Id == meetingId).Select(x => x.Id).SingleOrDefault();
            if (id == null)
            {
                return this.Redirect("/").WithDanger("Невалидни данни!");
            }

            return this.View();
        }

        public IActionResult UserConfig(string meetingId)
        {
            string userName = this.User.Identity.Name;

            return this.Json(this.videoChat.GetUserConfigurations(userName, meetingId, this.Request.Host.Value));
        }
    }
}
