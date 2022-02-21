namespace StudentsHelper.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Chat;
    using StudentsHelper.Web.Infrastructure.Alerts;

    public class ChatController : BaseController
    {
        private readonly IChatService chatService;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(
            IChatService chatService,
            UserManager<ApplicationUser> userManager)
        {
            this.chatService = chatService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> StartChat(string teacherUserId)
        {
            var userId = (await this.userManager.GetUserAsync(this.User)).Id;
            var chatGroupId = this.chatService.GetChatGroupByUsers(userId, teacherUserId);

            if (chatGroupId == null)
            {
                chatGroupId = await this.chatService.CreateChatGroupAsync(userId, teacherUserId);
            }

            return this.RedirectToAction(nameof(this.Chat), new { chatGroupId });
        }

        public async Task<IActionResult> Chat(string chatGroupId)
        {
            var userId = (await this.userManager.GetUserAsync(this.User)).Id;

            if (chatGroupId != null && !this.chatService.IsUserPartOfChatGroup(userId, chatGroupId))
            {
                return this.Redirect("/").WithDanger("Не сте част от тази група");
            }

            var viewModel = this.chatService.GetChatViewModel(userId, chatGroupId);

            return this.View(viewModel);
        }

        public IActionResult GetSenderProfilePicture(string picturePath, string email)
        {
            return this.ViewComponent("UserProfilePicture", new { picturePath, email });
        }
    }
}
