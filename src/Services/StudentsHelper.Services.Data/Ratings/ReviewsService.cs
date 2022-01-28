namespace StudentsHelper.Services.Data.Ratings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Ratings.Models;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Mapping;

    public class ReviewsService : IReviewsService
    {
        private readonly ITeachersService teachersService;
        private readonly IDeletableEntityRepository<Review> reviewsRepository;

        public ReviewsService(
            IDeletableEntityRepository<Review> reviewsRepository,
            ITeachersService teachersService)
        {
            this.reviewsRepository = reviewsRepository;
            this.teachersService = teachersService;
        }

        public IEnumerable<TeacherWithRating> GetTeachersRating(IQueryable<Teacher> teachers)
        {
            return teachers
                .Select(x => new TeacherWithRating
                {
                    Id = x.Id,
                    HourWage = x.HourWage.Value,
                    ApplicationUserEmail = x.ApplicationUser.Email,
                    ApplicationUserName = x.ApplicationUser.Name,
                    ApplicationUserPicturePath = x.ApplicationUser.PicturePath,
                    AverageRating = x.Reviews.Count != 0 ? Math.Round(x.Reviews.Average(x => x.Rating), 2) : 0,
                })
                .ToList();
        }

        public TeacherDetails GetTeacherRating(string teacherId)
        {
            return this.teachersService
                .GetAllAsNoTracking()
                .Where(x => x.Id == teacherId)
                .Select(x => new TeacherDetails
                {
                    Id = x.Id,
                    ApplicationUserName = x.ApplicationUser.Name,
                    ApplicationUserPicturePath = x.ApplicationUser.PicturePath,
                    HourWage = x.HourWage,
                    RatingRangesCount = new int[]
                    {
                        GetReviewsTypeCount(x.Reviews, 1),
                        GetReviewsTypeCount(x.Reviews, 2),
                        GetReviewsTypeCount(x.Reviews, 3),
                        GetReviewsTypeCount(x.Reviews, 4),
                        GetReviewsTypeCount(x.Reviews, 5),
                    },
                    AverageRating = x.Reviews.Count != 0 ? Math.Round(x.Reviews.Average(x => x.Rating), 2) : 0,
                })
                .SingleOrDefault();
        }

        public async Task AddReviewAsync(string teacherId, string studentId, byte rating, string comment)
        {
            await this.reviewsRepository.AddAsync(new Review
            {
                TeacherId = teacherId,
                StudentId = studentId,
                Rating = rating,
                Comment = comment,
            });

            await this.reviewsRepository.SaveChangesAsync();
        }

        /// <summary>
        /// Takes student id and teacher id.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="teacherId"></param>
        /// <returns>Returns true if has already reviewed and false if hasn't.</returns>
        public bool HasStudentReviewedTeacher(string studentId, string teacherId)
        {
            var responce = this.reviewsRepository
                .AllAsNoTracking()
                .Where(x => x.StudentId == studentId && x.TeacherId == teacherId)
                .Count();

            return responce > 0;
        }

        public IQueryable<T> GetAllReviewsForTeacher<T>(string teacherId)
        {
            return this.reviewsRepository
                .AllAsNoTracking()
                .Where(x => x.TeacherId == teacherId)
                .To<T>();
        }

        private static int GetReviewsTypeCount(ICollection<Review> reviews, int type)
        {
            return reviews.Select(x => x.Rating).Where(x => x == type).Count();
        }
    }
}
