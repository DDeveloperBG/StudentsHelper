namespace StudentsHelper.Web.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

#pragma warning disable SA1649 // File name should match first type name
    public class ShowRecoveryCodesModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        [TempData]
        public string[] RecoveryCodes { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            if (this.RecoveryCodes == null || this.RecoveryCodes.Length == 0)
            {
                return this.RedirectToPage("./TwoFactorAuthentication");
            }

            return this.Page();
        }
    }
}
