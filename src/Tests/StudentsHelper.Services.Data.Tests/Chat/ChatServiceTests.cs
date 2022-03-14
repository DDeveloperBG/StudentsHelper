namespace StudentsHelper.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Chat;
    using StudentsHelper.Web.ViewModels.Chat;

    using Xunit;

    public class ChatServiceTests : BaseTest
    {
        private List<ChatGroup> chatGroupsList;
        private IChatService chatService;

        [Theory]
        [InlineData(new object[] { new string[] { "1", "2" } })]
        [InlineData(new object[] { new string[] { "1", "2", "3" } })]
        public void CreateChatGroup_AddsGroupAndReturnsChatGroupId(string[] usersIds)
        {
            var chatGroupId = this.chatService.CreateChatGroupAsync(usersIds).GetAwaiter().GetResult();
            Assert.NotNull(chatGroupId);

            var addedChatGroup = this.chatGroupsList.First(x => x.Id == chatGroupId);
            Assert.NotNull(chatGroupId);

            Assert.Empty(addedChatGroup.Messages);
            Assert.NotEmpty(addedChatGroup.Users);

            // Assert all users are added
            Assert.True(usersIds
                .All(x => addedChatGroup.Users
                    .Any(y => y.ApplicationUserId == x && y.ChatGroupId == addedChatGroup.Id)));
        }

        [Fact]
        public void SaveMessageAsync_AddsMessageAndReturnsChatGroupId()
        {
            // Tested Method Parameters
            const string messageText = "Proba 123";
            const string senderUserId = "1";
            var sendTimeUtc = DateTime.UtcNow;

            string[] usersIds = new string[] { senderUserId, "2" };
            var chatGroup = new ChatGroup();
            chatGroup.Users = usersIds
                    .Select(x => new ChatGroupUsers
                    {
                        ApplicationUserId = x,
                        ChatGroupId = chatGroup.Id,
                    })
                    .ToList();
            this.chatGroupsList.Add(chatGroup);

            var modelInput = new MessageInputModel
            {
                GroupId = chatGroup.Id,
                Text = messageText,
            };

            // Call Tested Method
            var groupId = this.chatService.SaveMessageAsync(modelInput, senderUserId, sendTimeUtc).GetAwaiter().GetResult();
            Assert.Equal(chatGroup.Id, groupId);

            var message = chatGroup.Messages.First();

            Assert.Equal(messageText, message.Text);
            Assert.Equal(senderUserId, message.SenderId);
            Assert.Equal(sendTimeUtc, chatGroup.LastMessageTime);
        }

        [Fact]
        public void SaveMessagesAsync_DoNotAddMessageForNotPartOfGroupUser()
        {
            // Tested Method Parameters
            const string messageText = "Proba 123";
            const string senderUserId = "1";
            var sendTimeUtc = DateTime.UtcNow;

            string[] usersIds = new string[] { "2" };
            var chatGroup = new ChatGroup();
            chatGroup.Users = usersIds
                    .Select(x => new ChatGroupUsers
                    {
                        ApplicationUserId = x,
                        ChatGroupId = chatGroup.Id,
                    })
                    .ToList();
            this.chatGroupsList.Add(chatGroup);

            var modelInput = new MessageInputModel
            {
                GroupId = chatGroup.Id,
                Text = messageText,
            };

            // Call Tested Method
            var groupId = this.chatService.SaveMessageAsync(modelInput, senderUserId, sendTimeUtc).GetAwaiter().GetResult();
            Assert.Null(groupId);
            Assert.NotEqual(chatGroup.Id, groupId);

            Assert.Empty(chatGroup.Messages);
        }

        [Fact]
        public void SaveMessageAsync_AddsMessagesAndReturnsChatGroupId()
        {
            var sendTimeUtc = DateTime.UtcNow;
            const string messageText1 = "hello 1!";
            const string messageText2 = "hello 2!";

            string[] usersIds = new string[] { "1", "2" };
            var chatGroup = new ChatGroup();
            chatGroup.Users = usersIds
                    .Select(x => new ChatGroupUsers
                    {
                        ApplicationUserId = x,
                        ChatGroupId = chatGroup.Id,
                    })
                    .ToList();
            this.chatGroupsList.Add(chatGroup);

            var input1 = new MessageInputModel
            {
                GroupId = chatGroup.Id,
                Text = messageText1,
            };

            var input2 = new MessageInputModel
            {
                GroupId = chatGroup.Id,
                Text = messageText2,
            };

            // Call Tested Method
            var groupId = this.chatService.SaveMessageAsync(input1, usersIds[0], sendTimeUtc).GetAwaiter().GetResult();
            Assert.Equal(chatGroup.Id, groupId);

            var message1 = chatGroup.Messages.First();

            Assert.Equal(messageText1, message1.Text);
            Assert.Equal(usersIds[0], message1.SenderId);
            Assert.Equal(sendTimeUtc, chatGroup.LastMessageTime);

            groupId = this.chatService.SaveMessageAsync(input2, usersIds[1], sendTimeUtc).GetAwaiter().GetResult();
            Assert.Equal(chatGroup.Id, groupId);

            var message2 = chatGroup.Messages.Last();

            Assert.Equal(messageText2, message2.Text);
            Assert.Equal(usersIds[1], message2.SenderId);
            Assert.Equal(sendTimeUtc, chatGroup.LastMessageTime);
        }

        [Theory]
        [InlineData("1", true)]
        [InlineData("2", true)]
        [InlineData("3", false)]
        [InlineData("4", false)]
        [InlineData("4000", false)]
        public void IsUserPartOfChatGroup_ReturnsBoolIndicatingIsPartOfChatGroup(string userId, bool expectedBool)
        {
            string[] usersIds = new string[] { "1", "2" };
            var chatGroup = new ChatGroup();
            chatGroup.Users = usersIds
                    .Select(x => new ChatGroupUsers
                    {
                        ApplicationUserId = x,
                        ChatGroupId = chatGroup.Id,
                    })
                    .ToList();
            this.chatGroupsList.Add(chatGroup);

            Assert.Equal(expectedBool, this.chatService.IsUserPartOfChatGroup(userId, chatGroup.Id));
        }

        [Theory]
        [InlineData(new object[] { new string[] { "1", "2" } })]
        [InlineData(new object[] { new string[] { "1", "2", "3" } })]
        public void GetChatGroupByUsers_ReturnsGroupIdWhereUsersArePart(string[] usersIds)
        {
            var chatGroup = new ChatGroup();
            chatGroup.Users = usersIds
                    .Select(x => new ChatGroupUsers
                    {
                        ApplicationUserId = x,
                        ChatGroupId = chatGroup.Id,
                    })
                    .ToList();
            this.chatGroupsList.Add(chatGroup);

            var chatGroupId = this.chatService.GetChatGroupByUsers(usersIds);
            Assert.Equal(chatGroup.Id, chatGroupId);
        }

        [Theory]
        [InlineData(new object[] { new string[] { "1", "2" } })]
        [InlineData(new object[] { new string[] { "1", "2", "3" } })]
        public void GetChatGroupByUsers_ReturnsNullIfNoGroupIsFound(string[] usersIds)
        {
            var chatGroupId = this.chatService.GetChatGroupByUsers(usersIds);
            Assert.Null(chatGroupId);
        }

        [Fact]
        public void GetChatGroupViewModel_ReturnsChatGroupViewModel()
        {
            var user1 = new ApplicationUser
            {
                Name = "Gosho",
                Email = "Gosho@abv.bg",
                UserName = "Gosho@abv.bg",
                PicturePath = "/123/321",
            };
            var user2 = new ApplicationUser
            {
                Name = "Petur",
                Email = "Petur@abv.bg",
                UserName = "Petur@abv.bg",
                PicturePath = "/456/654",
            };
            var chatGroup = new ChatGroup();
            chatGroup.Users = new List<ChatGroupUsers>
                {
                    new ChatGroupUsers
                    {
                        ApplicationUser = user1,
                        ApplicationUserId = user1.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                    new ChatGroupUsers
                    {
                        ApplicationUser = user2,
                        ApplicationUserId = user2.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                };
            this.chatGroupsList.Add(chatGroup);

            var result1 = this.chatService.GetChatGroupViewModel(chatGroup.Id, user1.Id);
            Assert.Equal(chatGroup.Id, result1.Id);
            Assert.Equal(user2.Name, result1.OtherUserName);
            Assert.Equal(user2.Email, result1.OtherUserEmail);
            Assert.Equal(user2.PicturePath, result1.OtherUserPicturePath);

            var result2 = this.chatService.GetChatGroupViewModel(chatGroup.Id, user2.Id);
            Assert.Equal(chatGroup.Id, result2.Id);
            Assert.Equal(user1.Name, result2.OtherUserName);
            Assert.Equal(user1.Email, result2.OtherUserEmail);
            Assert.Equal(user1.PicturePath, result2.OtherUserPicturePath);
        }

        [Fact]
        public void GetAllUserGroups_ReturnsCollectionOfChatGroupViewModelContainingOneChatGroup()
        {
            var user1 = new ApplicationUser
            {
                Name = "Gosho",
                Email = "Gosho@abv.bg",
                PicturePath = "/123/321",
            };
            var user2 = new ApplicationUser
            {
                Name = "Petur",
                Email = "Petur@abv.bg",
                PicturePath = "/456/654",
            };
            var chatGroup = new ChatGroup();
            chatGroup.Users = new List<ChatGroupUsers>
                {
                    new ChatGroupUsers
                    {
                        ApplicationUser = user1,
                        ApplicationUserId = user1.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                    new ChatGroupUsers
                    {
                        ApplicationUser = user2,
                        ApplicationUserId = user2.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                };
            this.chatGroupsList.Add(chatGroup);

            var user1Groups = this.chatService.GetAllUserGroups(user1.Id);
            Assert.Single(user1Groups);

            var result1 = user1Groups.First();
            Assert.Equal(chatGroup.Id, result1.Id);
            Assert.Equal(user2.Name, result1.OtherUserName);
            Assert.Equal(user2.Email, result1.OtherUserEmail);
            Assert.Equal(user2.PicturePath, result1.OtherUserPicturePath);

            var user2Groups = this.chatService.GetAllUserGroups(user2.Id);
            Assert.Single(user2Groups);

            var result2 = user2Groups.First();
            Assert.Equal(chatGroup.Id, result2.Id);
            Assert.Equal(user1.Name, result2.OtherUserName);
            Assert.Equal(user1.Email, result2.OtherUserEmail);
            Assert.Equal(user1.PicturePath, result2.OtherUserPicturePath);
        }

        [Fact]
        public void GetAllUserGroups_ReturnsCollectionOfChatGroupViewModelContainingManyChatGroup()
        {
            var user1 = new ApplicationUser
            {
                Name = "Gosho",
                Email = "Gosho@abv.bg",
                PicturePath = "/123/321",
            };
            var user2 = new ApplicationUser
            {
                Name = "Petur",
                Email = "Petur@abv.bg",
                PicturePath = "/456/654",
            };
            var user3 = new ApplicationUser
            {
                Name = "Ivan",
                Email = "Ivan@abv.bg",
                PicturePath = "/789/987",
            };
            var chatGroup1 = new ChatGroup();
            chatGroup1.Users = new List<ChatGroupUsers>
            {
                new ChatGroupUsers
                {
                    ApplicationUser = user1,
                    ApplicationUserId = user1.Id,
                    ChatGroupId = chatGroup1.Id,
                },
                new ChatGroupUsers
                {
                    ApplicationUser = user2,
                    ApplicationUserId = user2.Id,
                    ChatGroupId = chatGroup1.Id,
                },
            };
            var chatGroup2 = new ChatGroup();
            chatGroup2.Users = new List<ChatGroupUsers>
            {
                new ChatGroupUsers
                {
                    ApplicationUser = user3,
                    ApplicationUserId = user3.Id,
                    ChatGroupId = chatGroup2.Id,
                },
                new ChatGroupUsers
                {
                    ApplicationUser = user1,
                    ApplicationUserId = user1.Id,
                    ChatGroupId = chatGroup2.Id,
                },
            };
            this.chatGroupsList.Add(chatGroup1);
            this.chatGroupsList.Add(chatGroup2);

            var user1Groups = this.chatService.GetAllUserGroups(user1.Id);

            var result1 = user1Groups.First();
            Assert.Equal(chatGroup1.Id, result1.Id);
            Assert.Equal(user2.Name, result1.OtherUserName);
            Assert.Equal(user2.Email, result1.OtherUserEmail);
            Assert.Equal(user2.PicturePath, result1.OtherUserPicturePath);

            var result2 = user1Groups.Last();
            Assert.Equal(chatGroup2.Id, result2.Id);
            Assert.Equal(user3.Name, result2.OtherUserName);
            Assert.Equal(user3.Email, result2.OtherUserEmail);
            Assert.Equal(user3.PicturePath, result2.OtherUserPicturePath);

            var user2Groups = this.chatService.GetAllUserGroups(user2.Id);
            Assert.Single(user2Groups);

            var result3 = user2Groups.First();
            Assert.Equal(chatGroup1.Id, result3.Id);
            Assert.Equal(user1.Name, result3.OtherUserName);
            Assert.Equal(user1.Email, result3.OtherUserEmail);
            Assert.Equal(user1.PicturePath, result3.OtherUserPicturePath);
        }

        [Fact]
        public void GetGroupMessages_ReturnsCollectionOfMessageViewModel()
        {
            var utcNow = DateTime.UtcNow;
            var sendTime1 = utcNow;
            var sendTime2 = utcNow.AddDays(1);

            var user1 = new ApplicationUser
            {
                Name = "Gosho",
                Email = "Gosho@abv.bg",
                PicturePath = "/123/321",
            };
            var user2 = new ApplicationUser
            {
                Name = "Petur",
                Email = "Petur@abv.bg",
                PicturePath = "/456/654",
            };
            var chatGroup = new ChatGroup();
            chatGroup.Users = new List<ChatGroupUsers>
                {
                    new ChatGroupUsers
                    {
                        ApplicationUser = user1,
                        ApplicationUserId = user1.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                    new ChatGroupUsers
                    {
                        ApplicationUser = user2,
                        ApplicationUserId = user2.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                };
            this.chatGroupsList.Add(chatGroup);

            var message1 = new Message
            {
                Sender = user1,
                SenderId = user1.Id,
                Text = "Hello1!!!",
                CreatedOn = sendTime1,
            };

            var message2 = new Message
            {
                Sender = user2,
                SenderId = user2.Id,
                Text = "Hello2!!!",
                CreatedOn = sendTime2,
            };

            chatGroup.Messages.Add(message1);
            chatGroup.Messages.Add(message2);

            var messages = this.chatService.GetGroupMessages(chatGroup.Id);
            Assert.Equal(2, messages.Count);

            var message1ViewModel = messages.First();
            Assert.Equal(user1.Email, message1ViewModel.SenderEmail);
            Assert.Equal(user1.Name, message1ViewModel.SenderName);
            Assert.Equal(user1.PicturePath, message1ViewModel.SenderPicturePath);
            Assert.Equal(sendTime1, message1ViewModel.SendTime);
            Assert.Equal(message1.Text, message1ViewModel.Text);

            var message2ViewModel = messages.Last();
            Assert.Equal(user2.Email, message2ViewModel.SenderEmail);
            Assert.Equal(user2.Name, message2ViewModel.SenderName);
            Assert.Equal(user2.PicturePath, message2ViewModel.SenderPicturePath);
            Assert.Equal(sendTime2, message2ViewModel.SendTime);
            Assert.Equal(message2.Text, message2ViewModel.Text);
        }

        [Fact]
        public void GetChatViewModel_ReturnsChatViewModel()
        {
            var utcNow = DateTime.UtcNow;
            var sendTime1 = utcNow;
            var sendTime2 = utcNow.AddDays(1);

            var user1 = new ApplicationUser
            {
                Name = "Gosho",
                Email = "Gosho@abv.bg",
                PicturePath = "/123/321",
            };
            var user2 = new ApplicationUser
            {
                Name = "Petur",
                Email = "Petur@abv.bg",
                PicturePath = "/456/654",
            };
            var chatGroup = new ChatGroup();
            chatGroup.Users = new List<ChatGroupUsers>
                {
                    new ChatGroupUsers
                    {
                        ApplicationUser = user1,
                        ApplicationUserId = user1.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                    new ChatGroupUsers
                    {
                        ApplicationUser = user2,
                        ApplicationUserId = user2.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                };
            this.chatGroupsList.Add(chatGroup);

            var message1 = new Message
            {
                Sender = user1,
                SenderId = user1.Id,
                Text = "Hello1!!!",
                CreatedOn = sendTime1,
            };

            var message2 = new Message
            {
                Sender = user2,
                SenderId = user2.Id,
                Text = "Hello2!!!",
                CreatedOn = sendTime2,
            };

            chatGroup.Messages.Add(message1);
            chatGroup.Messages.Add(message2);

            var chatViewModel = this.chatService.GetChatViewModel(user1.Id, chatGroup.Id);
            var messages = chatViewModel.Messages;
            Assert.Equal(2, messages.Count);

            var message1ViewModel = messages.First();
            Assert.Equal(user1.Email, message1ViewModel.SenderEmail);
            Assert.Equal(user1.Name, message1ViewModel.SenderName);
            Assert.Equal(user1.PicturePath, message1ViewModel.SenderPicturePath);
            Assert.Equal(sendTime1, message1ViewModel.SendTime);
            Assert.Equal(message1.Text, message1ViewModel.Text);

            var message2ViewModel = messages.Last();
            Assert.Equal(user2.Email, message2ViewModel.SenderEmail);
            Assert.Equal(user2.Name, message2ViewModel.SenderName);
            Assert.Equal(user2.PicturePath, message2ViewModel.SenderPicturePath);
            Assert.Equal(sendTime2, message2ViewModel.SendTime);
            Assert.Equal(message2.Text, message2ViewModel.Text);

            var user1Groups = chatViewModel.Groups;
            Assert.Single(user1Groups);

            var resultGroup = user1Groups.First();
            Assert.Equal(chatGroup.Id, resultGroup.Id);
            Assert.Equal(user2.Name, resultGroup.OtherUserName);
            Assert.Equal(user2.Email, resultGroup.OtherUserEmail);
            Assert.Equal(user2.PicturePath, resultGroup.OtherUserPicturePath);

            var resultSelectedGroup = chatViewModel.SelectedChatGroup;
            Assert.Equal(chatGroup.Id, resultSelectedGroup.Id);
            Assert.Equal(user2.Name, resultSelectedGroup.OtherUserName);
            Assert.Equal(user2.Email, resultSelectedGroup.OtherUserEmail);
            Assert.Equal(user2.PicturePath, resultSelectedGroup.OtherUserPicturePath);
        }

        [Fact]
        public void GetChatViewModel_ReturnsChatViewModelWhenGroupIdIsNull()
        {
            var utcNow = DateTime.UtcNow;
            var sendTime1 = utcNow;
            var sendTime2 = utcNow.AddDays(1);

            var user1 = new ApplicationUser
            {
                Name = "Gosho",
                Email = "Gosho@abv.bg",
                PicturePath = "/123/321",
            };
            var user2 = new ApplicationUser
            {
                Name = "Petur",
                Email = "Petur@abv.bg",
                PicturePath = "/456/654",
            };
            var chatGroup = new ChatGroup();
            chatGroup.Users = new List<ChatGroupUsers>
                {
                    new ChatGroupUsers
                    {
                        ApplicationUser = user1,
                        ApplicationUserId = user1.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                    new ChatGroupUsers
                    {
                        ApplicationUser = user2,
                        ApplicationUserId = user2.Id,
                        ChatGroupId = chatGroup.Id,
                    },
                };
            this.chatGroupsList.Add(chatGroup);

            var message1 = new Message
            {
                Sender = user1,
                SenderId = user1.Id,
                Text = "Hello1!!!",
                CreatedOn = sendTime1,
            };

            var message2 = new Message
            {
                Sender = user2,
                SenderId = user2.Id,
                Text = "Hello2!!!",
                CreatedOn = sendTime2,
            };

            chatGroup.Messages.Add(message1);
            chatGroup.Messages.Add(message2);

            var chatViewModel = this.chatService.GetChatViewModel(user1.Id, null);
            var messages = chatViewModel.Messages;
            Assert.Equal(2, messages.Count);

            var message1ViewModel = messages.First();
            Assert.Equal(user1.Email, message1ViewModel.SenderEmail);
            Assert.Equal(user1.Name, message1ViewModel.SenderName);
            Assert.Equal(user1.PicturePath, message1ViewModel.SenderPicturePath);
            Assert.Equal(sendTime1, message1ViewModel.SendTime);
            Assert.Equal(message1.Text, message1ViewModel.Text);

            var message2ViewModel = messages.Last();
            Assert.Equal(user2.Email, message2ViewModel.SenderEmail);
            Assert.Equal(user2.Name, message2ViewModel.SenderName);
            Assert.Equal(user2.PicturePath, message2ViewModel.SenderPicturePath);
            Assert.Equal(sendTime2, message2ViewModel.SendTime);
            Assert.Equal(message2.Text, message2ViewModel.Text);

            var user1Groups = chatViewModel.Groups;
            Assert.Single(user1Groups);

            var resultGroup = user1Groups.First();
            Assert.Equal(chatGroup.Id, resultGroup.Id);
            Assert.Equal(user2.Name, resultGroup.OtherUserName);
            Assert.Equal(user2.Email, resultGroup.OtherUserEmail);
            Assert.Equal(user2.PicturePath, resultGroup.OtherUserPicturePath);

            var resultSelectedGroup = chatViewModel.SelectedChatGroup;
            Assert.Equal(chatGroup.Id, resultSelectedGroup.Id);
            Assert.Equal(user2.Name, resultSelectedGroup.OtherUserName);
            Assert.Equal(user2.Email, resultSelectedGroup.OtherUserEmail);
            Assert.Equal(user2.PicturePath, resultSelectedGroup.OtherUserPicturePath);
        }

        [Fact]
        public void GetChatViewModel_ReturnsChatViewModelWhenGroupIdIsNullAndGroupsViewModelsIsNull()
        {
            const string userId = "1";

            var chatViewModel = this.chatService.GetChatViewModel(userId, null);

            Assert.Empty(chatViewModel.Groups);
            Assert.Empty(chatViewModel.Messages);
            Assert.Null(chatViewModel.SelectedChatGroup);
        }

        public override void CleanWorkbench()
        {
            this.chatGroupsList = new List<ChatGroup>();

            var repository = GetMockedClasses.MockIDeletableEntityRepository(this.chatGroupsList);

            this.chatService = new ChatService(repository.Object);
        }
    }
}
