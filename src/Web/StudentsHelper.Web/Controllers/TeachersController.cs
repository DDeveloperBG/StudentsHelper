namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.BusinessLogic.Teachers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Locations;

    public class TeachersController : BaseController
    {
        private readonly ITeachersBusinessLogicService teachersBusinessLogicService;

        public TeachersController(
            UserManager<ApplicationUser> userManager,
            ITeachersBusinessLogicService teachersBusinessLogicService)
            : base(userManager)
        {
            this.teachersBusinessLogicService = teachersBusinessLogicService;
        }

        public IActionResult All(
            int subjectId,
            LocationInputModel locationInputModel,
            int page = 1,
            string sortBy = "default",
            bool? isAscending = null)
        {
            var result = this
                .teachersBusinessLogicService
                .GetAllViewModel(subjectId, page, locationInputModel, sortBy, isAscending);

            if (result.ErrorMessage != null)
            {
                return this.Redirect("/").WithDanger(result.ErrorMessage);
            }

            this.SetSubjectIdInSession(subjectId);

            return this.View(result.ViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Details(string teacherId)
        {
            var user = await this.GetCurrentUserDataAsync();
            bool isUserStudent = this.User.IsInRole(GlobalConstants.StudentRoleName);

            var teacher = this
                .teachersBusinessLogicService
                .GetDetailsViewModel(teacherId, user.Id, isUserStudent);

            return this.View(teacher);
        }

        private void SetSubjectIdInSession(int subjectId)
        {
            this.HttpContext.Session.Set(GlobalConstants.SubjectIdSessionValueKey, BitConverter.GetBytes(subjectId));
        }
    }
}
