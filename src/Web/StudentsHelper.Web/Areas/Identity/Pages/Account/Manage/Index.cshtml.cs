namespace StudentsHelper.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Web.ViewModels.User;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ICloudStorageService cloudStorageService;
        private readonly IRepository<ApplicationUser> usersRepository;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ICloudStorageService cloudStorageService,
            IRepository<ApplicationUser> usersRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.cloudStorageService = cloudStorageService;
            this.usersRepository = usersRepository;
        }

        [TempData]
        public string ProfilePictureUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public UserProfileInfoModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            if (this.Input.ProfilePicture != null)
            {
                var profilePicPath = await this.cloudStorageService.SaveFileAsync(this.Input.ProfilePicture, GlobalConstants.ProfilePicturesFolder);
                var trackedUser = this.usersRepository.All().Where(x => x.Id == user.Id).SingleOrDefault();
                trackedUser.PicturePath = profilePicPath;
                await this.usersRepository.SaveChangesAsync();
            }

            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await this.userManager.SetPhoneNumberAsync(user, this.Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    this.StatusMessage = "Error: Неочаквана грешка при опит за задаване на телефонен номер.";
                    return this.RedirectToPage();
                }
            }

            if (this.Input.Name != user.Name)
            {
                try
                {
                    user.Name = this.Input.Name;
                    await this.usersRepository.SaveChangesAsync();
                }
                catch (System.Exception)
                {
                    this.StatusMessage = "Error: Неочаквана грешка при опит за задаване на телефонен номер.";
                    return this.RedirectToPage();
                }
            }

            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Вашият профил бе актуализиран";
            return this.RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);
            var profilePicUrl = this.cloudStorageService.GetImageUri(user.PicturePath, 130, 130);

            this.ProfilePictureUrl = profilePicUrl;

            this.Input = new UserProfileInfoModel
            {
                PhoneNumber = phoneNumber,
                Name = user.Name,
            };
        }
    }
}
