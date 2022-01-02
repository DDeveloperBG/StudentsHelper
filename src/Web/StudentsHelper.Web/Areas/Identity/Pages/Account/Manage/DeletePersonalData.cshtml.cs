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
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;

    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<DeletePersonalDataModel> logger;
        private readonly IDeletableEntityRepository<Teacher> teachersRepository;
        private readonly IDeletableEntityRepository<Student> studentsRepository;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IDeletableEntityRepository<Teacher> teacher,
            IDeletableEntityRepository<Student> student)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.teachersRepository = teacher;
            this.studentsRepository = student;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

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

            try
            {
                if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.TeacherRoleName);
                    var teacher = this.teachersRepository.AllAsNoTracking().Where(x => x.ApplicationUser == user).SingleOrDefault();
                    this.teachersRepository.HardDelete(teacher);
                }
                else if (this.User.IsInRole(GlobalConstants.StudentRoleName))
                {
                    await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.StudentRoleName);
                    var student = this.studentsRepository.AllAsNoTracking().Where(x => x.ApplicationUser == user).SingleOrDefault();
                    this.studentsRepository.HardDelete(student);
                }

                var result = await this.userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception();
                }

                await this.teachersRepository.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{user.Id}'.");
            }

            await this.signInManager.SignOutAsync();

            this.logger.LogInformation("User with ID '{UserId}' deleted themselves.", user.Id);

            return this.Redirect("~/");
        }
    }
}
