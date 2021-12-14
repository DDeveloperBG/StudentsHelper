namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.VideoChat;

    public class VideoChatController : Controller
    {
        private readonly ITwilioVideoChat twilioVideoChat;

        public VideoChatController(ITwilioVideoChat twilioVideoChat)
        {
            this.twilioVideoChat = twilioVideoChat;
        }

        public ActionResult VideoChat()
        {
            return this.View();
        }

        public ActionResult GetRoomAuthTocken()
        {
            string userId = this.User.Identity.Name;

            return this.Json(this.twilioVideoChat.CreateAccessTocken(userId));
        }
    }
}
