namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Common;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Location;
    using StudentsHelper.Services.Data.Paging.NewPaging;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.Ratings.Models;
    using StudentsHelper.Services.Data.SchoolSubjects;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.HtmlSanitizer;
    using StudentsHelper.Services.Mapping;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.ViewModels.Locations;
    using StudentsHelper.Web.ViewModels.Teachers;

    public class TeachersBusinessLogicService : ITeachersBusinessLogicService
    {
        private readonly ITeachersService teachersService;
        private readonly IStudentsService studentsService;
        private readonly ISchoolSubjectsService schoolSubjectsService;
        private readonly IReviewsService reviewsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILocationService locationService;
        private readonly IPagingService pagingService;
        private readonly IHtmlSanitizerService htmlSanitizerService;

        public TeachersBusinessLogicService(
            ITeachersService teachersService,
            IStudentsService studentsService,
            ISchoolSubjectsService schoolSubjectsService,
            IReviewsService reviewsService,
            ICloudStorageService cloudStorageService,
            IDateTimeProvider dateTimeProvider,
            ILocationService locationService,
            IPagingService pagingService,
            IHtmlSanitizerService htmlSanitizerService)
        {
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.schoolSubjectsService = schoolSubjectsService;
            this.reviewsService = reviewsService;
            this.cloudStorageService = cloudStorageService;
            this.dateTimeProvider = dateTimeProvider;
            this.locationService = locationService;
            this.pagingService = pagingService;
            this.htmlSanitizerService = htmlSanitizerService;
        }

        public (string ErrorMessage, TeachersOfSubjectType<TeacherWithRating> ViewModel) GetAllViewModel(
            int subjectId,
            int page,
            LocationInputModel locationInputModel,
            string sortBy,
            bool? isAscending)
        {
            var subjectName = this.schoolSubjectsService.GetSubjectName(subjectId);
            if (subjectName == null)
            {
                return (ValidationConstants.GeneralError, null);
            }

            IQueryable<TeacherWithRating> teachersAsQueryable;
            if (locationInputModel.RegionId > 0)
            {
                teachersAsQueryable = this.GetTeachersOfSubjectTypeWithRatingInLocation(subjectId, locationInputModel);
            }
            else
            {
                teachersAsQueryable = this.GetTeachersOfSubjectTypeWithRating(subjectId);
            }

            teachersAsQueryable = this.OrderByCriteria(teachersAsQueryable, sortBy, isAscending ?? true);
            var teachers = this.pagingService.GetPaged(teachersAsQueryable, page, 10);

            this.SetTeachersIsActiveState(teachers.Results, this.dateTimeProvider.GetUtcNow());

            var viewModel = new TeachersOfSubjectType<TeacherWithRating>
            {
                Teachers = teachers,
                SubjectId = subjectId,
                SubjectName = subjectName,
            };

            return (null, viewModel);
        }

        public TeacherDetails GetDetailsViewModel(string teacherId, string userId, bool isUserStudent)
        {
            var teacher = this.reviewsService.GetTeacherRating(teacherId);
            teacher.ApplicationUserPicturePath = this.cloudStorageService
                .GetImageUri(teacher.ApplicationUserPicturePath, 100, 100);

            if (isUserStudent)
            {
                var studentId = this.studentsService.GetId(userId);
                var hasAlreadyReviewed = this.reviewsService.HasStudentReviewedTeacher(studentId, teacherId);

                teacher.HasUserReviewedTeacher = hasAlreadyReviewed;
            }
            else
            {
                teacher.HasUserReviewedTeacher = true;
            }

            return teacher;
        }

        public Task UpdateDescription(string userId, string description)
        {
            var sanitizedDescription = this.htmlSanitizerService.SanitizeHtml(description);

            return this.teachersService.UpdateDescription(userId, sanitizedDescription);
        }

        public TeacherDescriptionViewModel GetDescriptionViewModel(string userId)
        {
            return this
                .teachersService
                .GetAll()
                .Where(x => x.ApplicationUserId == userId)
                .To<TeacherDescriptionViewModel>()
                .SingleOrDefault();
        }

        public string GetTeacherId(string userId)
        {
            return this.teachersService.GetId(userId);
        }

        private static bool IsTeacherActive(DateTime utcNow, DateTime lastTimeActive)
        {
            if (lastTimeActive == default(DateTime))
            {
                return false;
            }

            return (utcNow - lastTimeActive).TotalMinutes < 2;
        }

        private IQueryable<TeacherWithRating> OrderByCriteria(
            IQueryable<TeacherWithRating> teachers,
            string sortBy,
            bool isAscending)
        {
            if (isAscending)
            {
                switch (sortBy)
                {
                    case "name": return teachers.OrderBy(x => x.ApplicationUserName);
                    case "hourWage": return teachers.OrderBy(x => x.HourWage);
                    case "rating": return teachers.OrderBy(x => x.AverageRating);
                }
            }

            switch (sortBy)
            {
                case "name": return teachers.OrderByDescending(x => x.ApplicationUserName);
                case "hourWage": return teachers.OrderByDescending(x => x.HourWage);
                case "rating": return teachers.OrderByDescending(x => x.AverageRating);
            }

            return this.OrderByDefaultCriteria(teachers);
        }

        private IQueryable<TeacherWithRating> OrderByDefaultCriteria(
            IQueryable<TeacherWithRating> teachers)
        {
            return teachers
                .OrderByDescending(x => x.AverageRating)
                .ThenBy(x => x.HourWage);
        }

        private IQueryable<TeacherWithRating> GetTeachersOfSubjectTypeWithRating(int subjectId)
        {
            return this
                .reviewsService
                .GetTeachersRating(
                    this.teachersService
                        .GetAllOfType(subjectId));
        }

        private IQueryable<TeacherWithRating> GetTeachersOfSubjectTypeWithRatingInLocation(
            int subjectId,
            LocationInputModel locationInputModel)
        {
            return this.reviewsService
                .GetTeachersRating(
                    this.teachersService.GetAllOfType(
                        subjectId,
                        this.locationService.GetTeachersInLocation(
                            locationInputModel.RegionId,
                            locationInputModel.TownshipId,
                            locationInputModel.PopulatedAreaId,
                            locationInputModel.SchoolId)));
        }

        private void SetTeachersIsActiveState(IEnumerable<TeacherWithRating> teachers, DateTime utcNow)
        {
            foreach (var teacher in teachers)
            {
                GlobalVariables.UsersActivityDictionary
                    .TryGetValue(teacher.ApplicationUserEmail, out DateTime lastTimeActive);

                teacher.IsActive = IsTeacherActive(utcNow, lastTimeActive);
            }
        }
    }
}
