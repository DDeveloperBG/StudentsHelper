namespace StudentsHelper.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Data.Models;

    public class BaseController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public BaseController()
        {
        }

        public BaseController(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public Task<ApplicationUser> GetCurrentUserDataAsync()
        {
            return this.userManager.GetUserAsync(this.User);
        }
    }
}
