namespace StudentsHelper.Services.BusinessLogic.Teachers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.CloudStorage;
    using StudentsHelper.Services.Data.Location;
    using StudentsHelper.Services.Data.Paging.NewPaging;
    using StudentsHelper.Services.Data.Ratings;
    using StudentsHelper.Services.Data.Ratings.Models;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Time;
    using StudentsHelper.Web.ViewModels.Locations;
    using StudentsHelper.Web.ViewModels.Teachers;

    public class TeachersBusinessLogicService : ITeachersBusinessLogicService
    {
        private readonly ITeachersService teachersService;
        private readonly IStudentsService studentsService;
        private readonly IRepository<SchoolSubject> schoolSubjects;
        private readonly IReviewsService reviewsService;
        private readonly ICloudStorageService cloudStorageService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILocationService locationService;
        private readonly IPagingService pagingService;

        public TeachersBusinessLogicService(
            ITeachersService teachersService,
            IStudentsService studentsService,
            IRepository<SchoolSubject> schoolSubjects,
            IReviewsService reviewsService,
            ICloudStorageService cloudStorageService,
            IDateTimeProvider dateTimeProvider,
            ILocationService locationService,
            IPagingService pagingService)
        {
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.schoolSubjects = schoolSubjects;
            this.reviewsService = reviewsService;
            this.cloudStorageService = cloudStorageService;
            this.dateTimeProvider = dateTimeProvider;
            this.locationService = locationService;
            this.pagingService = pagingService;
        }

        public (string ErrorMessage, TeachersOfSubjectType<TeacherWithRating> ViewModel) GetAllViewModel(
            int subjectId,
            int page,
            LocationInputModel locationInputModel)
        {
            var subjectName = this.GetSubjectName(subjectId);
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

            var teachers = this.pagingService.GetPaged(teachersAsQueryable, page, 10);
            teachers.Results = this.OrderByDefaultCriteria(teachers.Results).ToList();

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

        private IEnumerable<TeacherWithRating> OrderByDefaultCriteria(
           IEnumerable<TeacherWithRating> teachers)
        {
            return teachers
                .OrderByDescending(x => x.IsActive)
                .ThenByDescending(x => x.AverageRating)
                .ThenBy(x => x.HourWage);
        }

        private IQueryable<TeacherWithRating> GetTeachersOfSubjectTypeWithRating(int subjectId)
        {
            return this
                .reviewsService
                .GetTeachersRating(this
                    .teachersService
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

        private string GetSubjectName(int subjectId)
        {
            return this
                .schoolSubjects
                .AllAsNoTracking()
                .Where(x => x.Id == subjectId)
                .Select(x => x.Name)
                .SingleOrDefault();
        }

        private void SetTeachersIsActiveState(IEnumerable<TeacherWithRating> teachers, DateTime utcNow)
        {
            foreach (var teacher in teachers)
            {
                GlobalVariables.UsersActivityDictionary
                    .TryGetValue(teacher.ApplicationUserEmail, out DateTime lastTimeActive);

                teacher.IsActive = this.IsTeacherActive(utcNow, lastTimeActive);
            }
        }

        private bool IsTeacherActive(DateTime utcNow, DateTime lastTimeActive)
        {
            if (lastTimeActive == default(DateTime))
            {
                return false;
            }
            else
            {
                return (utcNow - lastTimeActive).TotalMinutes < 2;
            }
        }
    }
}
