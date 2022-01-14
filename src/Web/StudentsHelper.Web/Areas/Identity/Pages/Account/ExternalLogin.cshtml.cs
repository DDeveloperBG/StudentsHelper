namespace StudentsHelper.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using IdentityModel;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Auth;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Web.Infrastructure.Alerts;

    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private const string UserRoleKey = "userRole";

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITeacherRegisterer teacherRegister;
        private readonly IStudentRegisterer studentRegisterer;
        private readonly ICloudStorageService cloudStorageService;
        private readonly ILogger<ExternalLoginModel> logger;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            ITeacherRegisterer teacherRegister,
            IStudentRegisterer studentRegisterer,
            ICloudStorageService cloudStorageService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.teacherRegister = teacherRegister;
            this.studentRegisterer = studentRegisterer;
            this.cloudStorageService = cloudStorageService;
        }

        [BindProperty]
        public TeacherInputModel TeacherModel { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGetAsync()
        {
            return this.RedirectToPage("./Login");
        }

        public IActionResult OnPost(string userRole, string provider, string returnUrl = null)
        {
            if (userRole != null)
            {
                this.HttpContext.Session.Set(UserRoleKey, Encoding.UTF8.GetBytes(userRole));
            }

            // Request a redirect to the external login provider.
            var redirectUrl = this.Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (remoteError != null)
            {
                this.ErrorMessage = $"Error from external provider: {remoteError}";
                return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl });
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                this.ErrorMessage = "Error loading external login information.";
                return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await this.signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                this.logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return this.LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                this.ReturnUrl = returnUrl;
                this.ProviderDisplayName = info.ProviderDisplayName;

                this.HttpContext.Session.TryGetValue(UserRoleKey, out byte[] roleBytes);

                // if rolebytes == null so it came from login page
                if (roleBytes == null)
                {
                    return await this.OnPostConfirmationAsync();
                }

                var role = Encoding.UTF8.GetString(roleBytes);

                if (role == GlobalConstants.StudentRoleName)
                {
                    return await this.OnPostConfirmationAsync();
                }

                return this.Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");

            // Get the information about the user from the external login provider
            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl }).WithDanger("Настъпи грешка");
            }

            if (!(info.Principal.HasClaim(c => c.Type == ClaimTypes.Name) &&
                        info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)))
            {
                return this.RedirectToPage("./Register").WithDanger("Настъпи грешка");
            }

            if (this.TeacherModel == null || this.ModelState.IsValid)
            {
                string profilePicUrl = info.Principal.FindFirstValue(JwtClaimTypes.Picture);
                string picturePath = null;
                if (profilePicUrl != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    HttpClient client = new HttpClient();
                    var responce = await client.GetAsync(profilePicUrl);
                    var fileStream = await responce.Content.ReadAsStreamAsync();

                    await this.cloudStorageService.UploadImageAsync(GlobalConstants.ProfilePicturesFolder, fileName, fileStream);

                    picturePath = $"{GlobalConstants.ProfilePicturesFolder}/{fileName}";
                }

                string name = info.Principal.FindFirstValue(ClaimTypes.Name);
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = new ApplicationUser
                {
                    Name = name,
                    Email = email,
                    UserName = email,
                    PicturePath = picturePath,
                };

                var result = await this.userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await this.userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        this.HttpContext.Session.TryGetValue(UserRoleKey, out byte[] roleBytes);

                        if (roleBytes == null)
                        {
                            return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl }).WithDanger("Нямате съществуващ профил.");
                        }

                        var role = Encoding.UTF8.GetString(roleBytes);

                        if (role == GlobalConstants.TeacherRoleName)
                        {
                            if (this.TeacherModel.QualificationDocument == null)
                            {
                                this.ModelState.AddModelError(string.Empty, "Документа за квалификация е необходим!");

                                return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl });
                            }

                            try
                            {
                                await this.teacherRegister.RegisterAsync(this.TeacherModel, user);
                            }
                            catch (ArgumentException e)
                            {
                                this.ModelState.AddModelError(string.Empty, e.Message);

                                return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl });
                            }
                        }
                        else if (role == GlobalConstants.StudentRoleName)
                        {
                            await this.studentRegisterer.RegisterAsync(user);
                        }
                        else
                        {
                            this.ModelState.AddModelError(string.Empty, "Wrong role name!");

                            return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl });
                        }

                        this.logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return this.RedirectToPage("./RegisterConfirmation", new { Email = user.Email });
                        }

                        await this.signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            this.ProviderDisplayName = info.ProviderDisplayName;
            this.ReturnUrl = returnUrl;

            return this.RedirectToPage("./Register", new { ReturnUrl = returnUrl });
        }
    }
}
