namespace StudentsHelper.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Chat;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.Hubs.Contracts;
    using StudentsHelper.Web.ViewModels.Chat;

    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatService chatService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(
            IChatService chatService,
            IDateTimeProvider dateTimeProvider,
            UserManager<ApplicationUser> userManager)
        {
            this.chatService = chatService;
            this.dateTimeProvider = dateTimeProvider;
            this.userManager = userManager;
        }

        public async Task JoinGroup(string groopName)
        {
            var user = await this.userManager.GetUserAsync(this.Context.User);
            if (!this.chatService.IsUserPartOfChatGroup(user.Id, groopName))
            {
                throw new System.Exception("User is not part of current group!!");
            }

            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groopName);
        }

        public async Task SendMessage(MessageInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.Context.User);
            var utcNow = this.dateTimeProvider.GetUtcNow();
            var groupId = await this.chatService.SaveMessageAsync(input, user.Id, utcNow);

            if (groupId == null)
            {
                return;
            }

            var messageViewModel = new MessageViewModel
            {
                SenderName = user.Name,
                SenderEmail = user.Email,
                SenderPicturePath = user.PicturePath,
                SendTime = utcNow,
                Text = input.Text,
            };

            //await this.Clients.Group(groupId).ReceiveMessage(messageViewModel);
            await this.Clients.All.ReceiveMessage(messageViewModel);
        }
    }
}
