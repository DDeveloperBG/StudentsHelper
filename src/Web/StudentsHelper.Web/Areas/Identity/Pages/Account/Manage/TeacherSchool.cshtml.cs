namespace StudentsHelper.Web.Areas.Identity.Pages.Account.Manage
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Location;
    using StudentsHelper.Web.ViewModels.Locations;

    public partial class TeacherSchoolModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILocationService locationService;

        public TeacherSchoolModel(
            UserManager<ApplicationUser> userManager,
            ILocationService locationService)
        {
            this.userManager = userManager;
            this.locationService = locationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public LocationInputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            this.LoadInputModelData(user.Id);

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (this.Input.SchoolId < 1)
            {
                this.LoadInputModelData(user.Id);
                this.StatusMessage = "Невалидни данни";
                return this.Page();
            }

            try
            {
                await this.locationService.ChangeTeacherLocationAsync(user.Id, this.Input.SchoolId);
            }
            catch (System.Exception)
            {
                this.StatusMessage = "Error: Неочаквана грешка при опит за промяна на училище.";
                return this.RedirectToPage();
            }

            this.StatusMessage = "Вашето училище бе актуализирано";
            return this.RedirectToPage();
        }

        private void LoadInputModelData(string userId)
        {
            this.Input = this.locationService.GetTeacherLocation(userId);
        }
    }
}
