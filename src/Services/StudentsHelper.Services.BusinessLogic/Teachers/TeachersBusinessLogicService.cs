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

        public TeachersBusinessLogicService(
            ITeachersService teachersService,
            IStudentsService studentsService,
            IRepository<SchoolSubject> schoolSubjects,
            IReviewsService reviewsService,
            ICloudStorageService cloudStorageService,
            IDateTimeProvider dateTimeProvider,
            ILocationService locationService)
        {
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.schoolSubjects = schoolSubjects;
            this.reviewsService = reviewsService;
            this.cloudStorageService = cloudStorageService;
            this.dateTimeProvider = dateTimeProvider;
            this.locationService = locationService;
        }

        public (string ErrorMessage, TeachersOfSubjectType<TeacherWithRating> ViewModel) GetAllViewModel(
            int subjectId,
            LocationInputModel locationInputModel)
        {
            var subjectName = this.GetSubjectName(subjectId);
            if (subjectName == null)
            {
                return (ValidationConstants.GeneralError, null);
            }

            IEnumerable<TeacherWithRating> teachers;
            if (locationInputModel.RegionId > 0)
            {
                teachers = this.GetTeachersOfSubjectTypeWithRatingInLocation(subjectId, locationInputModel);
            }
            else
            {
                teachers = this.GetTeachersOfSubjectTypeWithRating(subjectId);
            }

            this.ChangeTeachersIsActiveState(teachers, this.dateTimeProvider.GetUtcNow());

            var viewModel = new TeachersOfSubjectType<TeacherWithRating>
            {
                Teachers = this.OrderByDefaultCriteria(teachers),
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

        private IEnumerable<TeacherWithRating> GetTeachersOfSubjectTypeWithRating(int subjectId)
        {
            return this
                .reviewsService
                .GetTeachersRating(this
                    .teachersService
                    .GetAllOfType(subjectId));
        }

        private IEnumerable<TeacherWithRating> GetTeachersOfSubjectTypeWithRatingInLocation(
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

        private void ChangeTeachersIsActiveState(IEnumerable<TeacherWithRating> teachers, DateTime utcNow)
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
