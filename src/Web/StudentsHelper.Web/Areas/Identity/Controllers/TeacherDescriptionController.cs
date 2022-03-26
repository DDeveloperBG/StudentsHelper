namespace StudentsHelper.Web.Areas.Identity.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.BusinessLogic.Teachers;
    using StudentsHelper.Web.Controllers;
    using StudentsHelper.Web.Infrastructure.Alerts;

    [Area("Identity")]
    [Authorize(Roles = GlobalConstants.TeacherRoleName)]
    public class TeacherDescriptionController : BaseController
    {
        private ITeachersBusinessLogicService teachersBusinessLogicService;

        public TeacherDescriptionController(
            ITeachersBusinessLogicService teachersBusinessLogicService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this.teachersBusinessLogicService = teachersBusinessLogicService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.GetCurrentUserDataAsync();

            var viewModel = this.teachersBusinessLogicService.GetDescriptionViewModel(user.Id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDesciption(string description)
        {
            if (!this.ModelState.IsValid)
            {
                return this.LocalRedirect("/Identity/TeacherDescription/Index")
                    .WithDanger(ValidationConstants.GeneralError);
            }

            var user = await this.GetCurrentUserDataAsync();

            await this.teachersBusinessLogicService.UpdateDescription(user.Id, description);

            var teacherId = this.teachersBusinessLogicService.GetTeacherId(user.Id);

            return this.Redirect($"/Teachers/Details?teacherId={teacherId}")
                .WithSuccess(GlobalConstants.GeneralMessages.TaskSucceededMessage);
        }
    }
}
