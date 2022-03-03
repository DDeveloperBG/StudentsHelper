namespace StudentsHelper.Services.Data.Chat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Web.ViewModels.Chat;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<ChatGroup> chatGroupRepository;

        public ChatService(IDeletableEntityRepository<ChatGroup> chatGroupRepository)
        {
            this.chatGroupRepository = chatGroupRepository;
        }

        public async Task<string> SaveMessageAsync(MessageInputModel input, string userId, DateTime utcNow)
        {
            if (!this.IsUserPartOfChatGroup(userId, input.GroupId))
            {
                return null;
            }

            var chatGroup = this.chatGroupRepository
                .All()
                .Where(x => x.Id == input.GroupId)
                .Single();

            var message = new Message
            {
                SenderId = userId,
                Text = input.Text,
            };

            chatGroup.LastMessageTime = utcNow;
            chatGroup.Messages.Add(message);
            await this.chatGroupRepository.SaveChangesAsync();

            return chatGroup.Id;
        }

        public bool IsUserPartOfChatGroup(string userId, string groupId)
        {
            return this.chatGroupRepository
                .AllAsNoTracking()
                .Where(x => x.Id == groupId && x.Users.Any(x => x.ApplicationUserId == userId))
                .Select(x => x.Id)
                .SingleOrDefault() == groupId;
        }

        public async Task<string> CreateChatGroupAsync(params string[] usersIds)
        {
            var chatGroup = new ChatGroup();

            foreach (var id in usersIds)
            {
                var groupUser = new ChatGroupUsers
                {
                    ApplicationUserId = id,
                    ChatGroupId = chatGroup.Id,
                };

                chatGroup.Users.Add(groupUser);
            }

            await this.chatGroupRepository.AddAsync(chatGroup);
            await this.chatGroupRepository.SaveChangesAsync();

            return chatGroup.Id;
        }

        public string GetChatGroupByUsers(params string[] usersIds)
        {
            return this.chatGroupRepository
                .AllAsNoTracking()
                .Where(x => x.Users.All(y => usersIds.Contains(y.ApplicationUserId)))
                .Select(x => x.Id)
                .SingleOrDefault();
        }

        public ChatGroupViewModel GetChatGroupViewModel(string groupId, string userId)
        {
            return this.chatGroupRepository
                .AllAsNoTracking()
                .Where(x => x.Id == groupId)
                .Select(x => new ChatGroupViewModel
                {
                    Id = x.Id,
                    OtherUserEmail = x.Users.First(x => x.ApplicationUserId != userId).ApplicationUser.Email,
                    OtherUserName = x.Users.First(x => x.ApplicationUserId != userId).ApplicationUser.Name,
                    OtherUserPicturePath = x.Users.First(x => x.ApplicationUserId != userId).ApplicationUser.PicturePath,
                })
                .Single();
        }

        public ICollection<ChatGroupViewModel> GetAllUserGroups(string userId)
        {
            return this.chatGroupRepository
                .AllAsNoTracking()
                .Where(x => x.Users.Any(x => x.ApplicationUserId == userId))
                .OrderByDescending(x => x.LastMessageTime)
                .Select(x => new ChatGroupViewModel
                {
                    Id = x.Id,
                    OtherUserEmail = x.Users.First(x => x.ApplicationUserId != userId).ApplicationUser.Email,
                    OtherUserName = x.Users.First(x => x.ApplicationUserId != userId).ApplicationUser.Name,
                    OtherUserPicturePath = x.Users.First(x => x.ApplicationUserId != userId).ApplicationUser.PicturePath,
                })
                .ToList();
        }

        public ICollection<MessageViewModel> GetGroupMessages(string groupId)
        {
            return this.chatGroupRepository
                .AllAsNoTracking()
                .Where(x => x.Id == groupId)
                .OrderBy(x => x.CreatedOn)
                .SelectMany(x => x.Messages.Select(y => new MessageViewModel
                {
                    SenderPicturePath = y.Sender.PicturePath,
                    SenderEmail = y.Sender.Email,
                    SenderName = y.Sender.Name,
                    SendTime = y.CreatedOn,
                    Text = y.Text,
                }))
                .ToList();
        }

        public ChatViewModel GetChatViewModel(string userId, string groupId)
        {
            var groupsViewModels = this.GetAllUserGroups(userId);

            if (groupId == null)
            {
                groupId = groupsViewModels.FirstOrDefault()?.Id;
            }

            ICollection<MessageViewModel> groupMessages = new List<MessageViewModel>();
            ChatGroupViewModel selectedChatGroup = null;

            if (groupId != null)
            {
                groupMessages = this.GetGroupMessages(groupId);
                selectedChatGroup = this.GetChatGroupViewModel(groupId, userId);
            }

            return new ChatViewModel
            {
                Groups = groupsViewModels,
                Messages = groupMessages,
                SelectedChatGroup = selectedChatGroup,
            };
        }
    }
}
