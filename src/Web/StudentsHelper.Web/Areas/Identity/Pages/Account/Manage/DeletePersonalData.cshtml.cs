namespace StudentsHelper.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using StudentsHelper.Common;
    using StudentsHelper.Data;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.Teachers;

#pragma warning disable SA1649 // File name should match first type name
    public class DeletePersonalDataModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly ITeachersService teachersService;
        private readonly IStudentsService studentsService;
        private readonly IRepository<ChatGroupUsers> chatGroupsRepository;
        private readonly IRepository<Message> messagesRepository;
        private readonly ApplicationDbContext dbContext;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            ITeachersService teachersService,
            IStudentsService studentsService,
            ApplicationDbContext dbContext,
            IRepository<ChatGroupUsers> chatGroupsRepository,
            IRepository<Message> messagesRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.dbContext = dbContext;
            this.chatGroupsRepository = chatGroupsRepository;
            this.messagesRepository = messagesRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            if (this.RequirePassword)
            {
                if (!await this.userManager.CheckPasswordAsync(user, this.Input.Password))
                {
                    this.ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return this.Page();
                }
            }

            await this.DeleteUser(user);

            return this.Redirect("~/");
        }

        private async Task DeleteUser(ApplicationUser user)
        {
            using (var transaction = this.dbContext.Database.BeginTransaction())
            {
                try
                {
                    await this.DeleteUserMessagesAndChatGroupsAsync(user.Id);

                    await this.DeleteUserAsTeacherOrStudent(user.Id);

                    await this.DeleteUserExternalLogins(user);

                    await this.DeleteUserTokens(user.Id);

                    var result = await this.userManager.DeleteAsync(user);
                    var userId = await this.userManager.GetUserIdAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
                    }

                    await this.signInManager.SignOutAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return;
                }

                transaction.Commit();
            }

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);
        }

        private async Task DeleteUserMessagesAndChatGroupsAsync(string userId)
        {
            var deletedUser = await this.userManager.FindByNameAsync(GlobalConstants.DeletedUserUsername);
            var allUserMessages = this.messagesRepository
                .All()
                .Where(x => x.SenderId == userId)
                .ToList();
            foreach (var message in allUserMessages)
            {
                message.SenderId = deletedUser.Id;
                message.Sender = deletedUser;
            }

            var allUserChatGroups = this.chatGroupsRepository
                .All()
                .Where(x => x.ApplicationUserId == userId)
                .ToList();
            foreach (var chatGroup in allUserChatGroups)
            {
                chatGroup.ApplicationUserId = deletedUser.Id;
                chatGroup.ApplicationUser = deletedUser;
            }

            await this.dbContext.SaveChangesAsync();
        }

        private async Task DeleteUserAsTeacherOrStudent(string userId)
        {
            if (this.User.IsInRole(GlobalConstants.StudentRoleName))
            {
                await this.studentsService.DeleteStudentAsync(userId);
            }
            else if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                await this.teachersService.DeleteTeacherAsync(userId);
            }
        }

        private async Task DeleteUserExternalLogins(ApplicationUser user)
        {
            var externalLogins = await this.userManager.GetLoginsAsync(user);

            foreach (var item in externalLogins)
            {
                await this.userManager.RemoveLoginAsync(user, item.LoginProvider, item.ProviderKey);
            }
        }

        private async Task DeleteUserTokens(string userId)
        {
            var userToken = this.dbContext
                    .UserTokens
                    .Where(x => x.UserId == userId)
                    .SingleOrDefault();

            if (userToken != null)
            {
                this.dbContext.UserTokens.Remove(userToken);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
