namespace StudentsHelper.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.ViewModels.Teachers;

    public class TeachersController : Controller
    {
        private readonly ITeachersService teachersService;

        public TeachersController(ITeachersService teachersService)
        {
            this.teachersService = teachersService;
        }

        public IActionResult All(int subjectId)
        {
            var teachers = this.teachersService.GetAllOfType<TeacherViewModel>(subjectId);

            return this.View(teachers);
        }
    }
}
