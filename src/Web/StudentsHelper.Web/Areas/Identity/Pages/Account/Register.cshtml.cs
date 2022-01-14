namespace StudentsHelper.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Auth;
    using StudentsHelper.Web.Infrastructure.Filters;

    [GuestFilter]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly ITeacherRegisterer teacherRegister;
        private readonly IStudentRegisterer studentRegisterer;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ITeacherRegisterer teacherRegister,
            IStudentRegisterer studentRegisterer)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.teacherRegister = teacherRegister;
            this.studentRegisterer = studentRegisterer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= this.Url.Content("~/");
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {
                if (this.Input.Role == GlobalConstants.TeacherRoleName)
                {
                    if (this.Input.TeacherModel.QualificationDocument == null)
                    {
                        this.ModelState.AddModelError(string.Empty, "Qualification document is required!");

                        return this.Page();
                    }
                }

                var user = new ApplicationUser { Name = this.Input.Name, Email = this.Input.Email, UserName = this.Input.Email };
                var result = await this.userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    if (this.Input.Role == GlobalConstants.TeacherRoleName)
                    {
                        try
                        {
                            await this.teacherRegister.RegisterAsync(this.Input.TeacherModel, user);
                        }
                        catch (ArgumentException e)
                        {
                            this.ModelState.AddModelError(string.Empty, e.Message);

                            return this.Page();
                        }
                    }
                    else if (this.Input.Role == GlobalConstants.StudentRoleName)
                    {
                        await this.studentRegisterer.RegisterAsync(user);
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, "Wrong role name!");

                        return this.Page();
                    }

                    this.logger.LogInformation("User created a new account with password.");

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("./RegisterConfirmation", new { Email = user.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }

    public class InputModel
    {
        [Display(Name = "Роля")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        public string Role { get; set; }

        [Display(Name = "Име")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        public string Name { get; set; }

        [Display(Name = "Имейл")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес")]
        public string Email { get; set; }

        [Display(Name = "Парола")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [StringLength(100, ErrorMessage = "Паролата трябва да бъде поне {2} и максимум {1} символа дълга.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public TeacherInputModel TeacherModel { get; set; }
    }
}
