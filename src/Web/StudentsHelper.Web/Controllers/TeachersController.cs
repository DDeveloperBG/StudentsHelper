namespace StudentsHelper.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.Teachers;

    public class TeachersController : Controller
    {
        private readonly ITeachersService teachersService;
        private readonly IStudentsService studentsService;
        private readonly IReviewsService reviewsService;
        private readonly UserManager<ApplicationUser> userManager;

        public TeachersController(
            ITeachersService teachersService,
            IStudentsService studentsService,
            IReviewsService reviewsService,
            UserManager<ApplicationUser> userManager)
        {
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.reviewsService = reviewsService;
            this.userManager = userManager;
        }

        public IActionResult All(int subjectId)
        {
            var teachers = this.reviewsService.GetTeachersRating(this.teachersService.GetAllOfType(subjectId));

            return this.View(teachers);
        }

        [Authorize]
        public async Task<IActionResult> Details(string teacherId)
        {
            if (!this.User.IsInRole(GlobalConstants.StudentRoleName))
            {
                this.TempData["HasAlreadyReviewed"] = true;
            }
            else
            {
                var user = await this.userManager.GetUserAsync(this.User);
                var studentId = this.studentsService.GetId(user.Id);
                var hasAlreadyReviewed = this.reviewsService.HasStudentReviewedTeacher(studentId, teacherId);
                this.TempData["HasAlreadyReviewed"] = hasAlreadyReviewed;
            }

            var teacher = this.reviewsService.GetTeacherRating(teacherId);

            return this.View(teacher);
        }
    }
}
