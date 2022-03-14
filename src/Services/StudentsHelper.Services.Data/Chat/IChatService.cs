namespace StudentsHelper.Services.Data.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Chat;

    public interface IChatService
    {
        Task<string> CreateChatGroupAsync(params string[] usersIds);

        Task<string> SaveMessageAsync(MessageInputModel input, string userId, DateTime utcNow);

        bool IsUserPartOfChatGroup(string userId, string groupId);

        string GetChatGroupByUsers(params string[] usersIds);

        ChatGroupViewModel GetChatGroupViewModel(string groupId, string userId);

        ICollection<ChatGroupViewModel> GetAllUserGroups(string userId);

        ICollection<MessageViewModel> GetGroupMessages(string groupId);

        ChatViewModel GetChatViewModel(string userId, string groupId);
    }
}
