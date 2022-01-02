namespace StudentsHelper.Web.Controllers
{
    using System;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.VideoChat;

    [Authorize]
    public class VideoChatController : Controller
    {
        private readonly IVideoChat videoChat;

        public VideoChatController(IVideoChat videoChat)
        {
            this.videoChat = videoChat;
        }

        public ActionResult VideoChat(string meetingId)
        {
            if (meetingId == null)
            {
                meetingId = Guid.NewGuid().ToString();
                return this.Redirect($"/VideoChat/VideoChat?{nameof(meetingId)}={meetingId}");
            }

            return this.View();
        }

        public ActionResult UserConfig(string meetingId)
        {
            string userName = this.User.Identity.Name;

            return this.Json(this.videoChat.GetUserConfigurations(userName, meetingId, this.Request.Host.Value));
        }
    }
}
