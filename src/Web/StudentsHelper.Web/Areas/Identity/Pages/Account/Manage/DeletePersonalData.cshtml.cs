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
    using StudentsHelper.Data.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

#pragma warning disable SA1649 // File name should match first type name
    public class DeletePersonalDataModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly IRepository<Teacher> teachersRepository;
        private readonly IRepository<Student> studentsRepository;
        private readonly ApplicationDbContext dbContext;
        private readonly IDbQueryRunner dbQueryRunner;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IRepository<Teacher> teachersRepository,
            IRepository<Student> studentsRepository,
            ApplicationDbContext dbContext,
            IDbQueryRunner dbQueryRunner)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.teachersRepository = teachersRepository;
            this.studentsRepository = studentsRepository;
            this.dbContext = dbContext;
            this.dbQueryRunner = dbQueryRunner;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.RequirePassword = await this.userManager.HasPasswordAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
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

            using (var transaction = this.dbContext.Database.BeginTransaction())
            {
                var deletedUser = await this.userManager.FindByNameAsync(GlobalConstants.DeletedUserUsername);

                if (this.User.IsInRole(GlobalConstants.StudentRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.StudentRoleName);
                    this.studentsRepository.All().Where(x => x.ApplicationUserId == user.Id).SingleOrDefault().ApplicationUser = deletedUser;
                    await this.studentsRepository.SaveChangesAsync();
                }
                else if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.TeacherRoleName);
                    this.teachersRepository.All().Where(x => x.ApplicationUserId == user.Id).SingleOrDefault().ApplicationUser = deletedUser;
                    await this.teachersRepository.SaveChangesAsync();
                }

                var externalLogins = await this.userManager.GetLoginsAsync(user);

                foreach (var item in externalLogins)
                {
                    await this.userManager.RemoveLoginAsync(user, item.LoginProvider, item.ProviderKey);
                }

                await this.dbQueryRunner.RunQueryAsync(
                    @"DELETE FROM [StudentsHelper].[dbo].[AspNetUserTokens] WHERE UserId = {0}",
                    user.Id);

                await this.dbContext.SaveChangesAsync();

                var result = await this.userManager.DeleteAsync(user);
                var userId = await this.userManager.GetUserIdAsync(user);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
                }

                await this.signInManager.SignOutAsync();

                transaction.Commit();
            }

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            return this.Redirect("~/");
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
