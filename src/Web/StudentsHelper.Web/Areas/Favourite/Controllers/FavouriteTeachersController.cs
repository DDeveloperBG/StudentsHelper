namespace StudentsHelper.Web.Areas.Favourite.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.BusinessLogic.StudentFavouriteTeachers;
    using StudentsHelper.Web.Controllers;

    [Area("Favourite")]
    public class FavouriteTeachersController : BaseController
    {
        private readonly IStudentFavouriteTeachersBusinessLogicService studentFavouriteTeachersBusinessLogicService;

        public FavouriteTeachersController(
            UserManager<ApplicationUser> userManager,
            IStudentFavouriteTeachersBusinessLogicService studentFavouriteTeachersBusinessLogicService)
            : base(userManager)
        {
            this.studentFavouriteTeachersBusinessLogicService = studentFavouriteTeachersBusinessLogicService;
        }

        public async Task<IActionResult> IndexAsync(int page = 1)
        {
            var studentUser = await this.GetCurrentUserDataAsync();

            var viewModel = this.studentFavouriteTeachersBusinessLogicService
                .GetIndexPageViewModel(studentUser.Id, page);

            return this.View(viewModel);
        }

        public async Task<IActionResult> AddOrRemoveAsync(string teacherUserId, string returnUrl)
        {
            var studentUser = await this.GetCurrentUserDataAsync();
            await this.studentFavouriteTeachersBusinessLogicService
                .AddOrRemoveTeacherAsync(studentUser.Id, teacherUserId);

            return this.Redirect(returnUrl);
        }
    }
}
