namespace StudentsHelper.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Web.ViewModels;
    using StudentsHelper.Web.ViewModels.SchoolSubjects;

    public class HomeController : BaseController
    {
        private readonly ISchoolSubjectsService schoolSubjectsService;

        public HomeController(ISchoolSubjectsService schoolSubjectsService)
        {
            this.schoolSubjectsService = schoolSubjectsService;
        }

        public IActionResult Index()
        {
            var schoolSubjects = this.schoolSubjectsService.GetAll<SchoolSubjectViewModel>();
            var model = new SchoolSubjectsListViewModel { SchoolSubjects = schoolSubjects };
            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

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
    }
}
