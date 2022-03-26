namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels;
    using StudentsHelper.Web.ViewModels.SchoolSubjects;

    public class HomeController : BaseController
    {
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITeachersService teachersService;

        public HomeController(
            ISchoolSubjectsService schoolSubjectsService,
            UserManager<ApplicationUser> userManager,
            ITeachersService teachersService)
        {
            this.schoolSubjectsService = schoolSubjectsService;
            this.userManager = userManager;
            this.teachersService = teachersService;
        }

        public async Task<IActionResult> IndexAsync(string message)
        {
            var schoolSubjects = this.schoolSubjectsService.GetAll<SchoolSubjectViewModel>();
            var model = new SchoolSubjectsListViewModel { SchoolSubjects = schoolSubjects };

            if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                var user = await this.userManager.GetUserAsync(this.User);
                var teacherId = this.teachersService.GetId(user.Id);
                var hourWage = this.teachersService.GetHourWage(teacherId);

                if (hourWage == null)
                {
                    return this.View(model).WithWarning(GlobalConstants.TeacherMessages.NotSetHourWage);
                }
            }

            IActionResult responce = this.View(model);
            if (message != null)
            {
                responce = responce.WithInfo(message);
            }

            return responce;
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        // Needs to be legally valid
        // public IActionResult Terms()
        // {
        //    return this.View();
        // }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue && statusCode.Value == 404)
            {
                return this.View("Error404");
            }

            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        [HttpGet("robots.txt")]
        [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
        public IActionResult RobotsTxt()
        {
            string content = "User-agent: *" + Environment.NewLine + "Disallow:";

            return this.Content(content);
        }
    }
}
