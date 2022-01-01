namespace StudentsHelper.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.ViewModels.Teachers;

    public class TeachersController : AdministrationController
    {
        private readonly ITeachersService teachersService;

        public TeachersController(ITeachersService teachersService)
        {
            this.teachersService = teachersService;
        }

        public IActionResult AllToApprove()
        {
            var teachers = this.teachersService.GetAllNotApproved<TeacherViewModel>();
            return this.View(teachers);
        }
    }
}
