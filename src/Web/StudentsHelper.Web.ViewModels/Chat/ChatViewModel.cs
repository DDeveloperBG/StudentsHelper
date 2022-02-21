namespace StudentsHelper.Web.ViewModels.Chat
{
    using System.Collections.Generic;

    public class ChatViewModel
    {
        public ChatGroupViewModel SelectedChatGroup { get; set; }

        public ICollection<ChatGroupViewModel> Groups { get; set; }

        public ICollection<MessageViewModel> Messages { get; set; }
    }
}
