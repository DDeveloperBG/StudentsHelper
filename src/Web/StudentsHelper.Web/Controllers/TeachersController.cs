﻿namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.Infrastructure.Alerts;

    public class TeachersController : Controller
    {
        private readonly ITeachersService teachersService;
        private readonly IStudentsService studentsService;
        private readonly IRepository<SchoolSubject> schoolSubjects;
        private readonly IReviewsService reviewsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDateTimeProvider dateTimeProvider;

        public TeachersController(
            ITeachersService teachersService,
            IStudentsService studentsService,
            IReviewsService reviewsService,
            ICloudStorageService cloudStorageService,
            IRepository<SchoolSubject> schoolSubjects,
            UserManager<ApplicationUser> userManager,
            IDateTimeProvider dateTimeProvider)
        {
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.schoolSubjects = schoolSubjects;
            this.reviewsService = reviewsService;
            this.cloudStorageService = cloudStorageService;
            this.userManager = userManager;
            this.dateTimeProvider = dateTimeProvider;
        }

        public IActionResult All(int subjectId)
        {
            var name = this.schoolSubjects.AllAsNoTracking().Where(x => x.Id == subjectId).Select(x => x.Name).SingleOrDefault();
            if (name == null)
            {
                return this.Redirect("/").WithDanger("Невалидни данни!");
            }

            this.HttpContext.Session.Set("subjectId", BitConverter.GetBytes(subjectId));

            DateTime utcNow = this.dateTimeProvider.GetUtcNow();
            DateTime emptyTime = default;
            var teachers = this.reviewsService.GetTeachersRating(this.teachersService.GetAllOfType(subjectId));
            foreach (var teacher in teachers)
            {
                GlobalVariables.UsersActivityDictionary
                    .TryGetValue(teacher.ApplicationUserEmail, out DateTime lastTimeActive);

                if (lastTimeActive == emptyTime)
                {
                    teacher.IsActive = false;
                }
                else
                {
                    teacher.IsActive = (utcNow - lastTimeActive).Minutes < 2;
                }
            }

            teachers = teachers.OrderByDescending(x => x.IsActive).ThenByDescending(x => x.AverageRating).ThenBy(x => x.HourWage);

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
            teacher.ApplicationUserPicturePath = this.cloudStorageService
                .GetImageUri(teacher.ApplicationUserPicturePath, 100, 100);

            return this.View(teacher);
        }
    }
}
