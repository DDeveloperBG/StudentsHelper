﻿namespace StudentsHelper.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Reviews;

    public class ReviewsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStudentsService studentsService;
        private readonly IReviewsService reviewsService;

        public ReviewsController(
            UserManager<ApplicationUser> userManager,
            IStudentsService studentsService,
            IReviewsService reviewsService)
        {
            this.userManager = userManager;
            this.studentsService = studentsService;
            this.reviewsService = reviewsService;
        }

        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        [HttpGet]
        public IActionResult ShowAddReview(ReviewInputModel input)
        {
            var redirect = this.RedirectToAction(nameof(TeachersController.Details), "Teachers", new { teacherId = input.TeacherId });

            if (!this.ModelState.IsValid)
            {
                return redirect.WithDanger("Невалидни данни.");
            }

            return this.View(input);
        }

        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewInputModel input)
        {
            var redirect = this.RedirectToAction(nameof(TeachersController.Details), "Teachers", new { teacherId = input.TeacherId });

            if (!this.ModelState.IsValid)
            {
                return redirect.WithDanger("Невалидни данни.");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var studentId = this.studentsService.GetId(user.Id);

            if (this.reviewsService.HasStudentReviewedTeacher(studentId, input.TeacherId))
            {
                return redirect.WithWarning("Вече сте оценили един път.");
            }

            await this.reviewsService.AddReviewAsync(input.TeacherId, studentId, input.Rating, input.Comment);

            return redirect.WithSuccess("Успешно дадохте оценка.");
        }
    }
}
