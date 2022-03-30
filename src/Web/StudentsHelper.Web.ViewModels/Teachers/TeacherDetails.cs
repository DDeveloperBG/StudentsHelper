namespace StudentsHelper.Services.Data.Ratings.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeacherDetails : IMapFrom<Teacher>, IHaveCustomMappings
    {
        private IList<int> ratingRangesCount;

        public string Id { get; set; }

        public bool IsFavourite { get; set; }

        public string Description { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserPicturePath { get; set; }

        public IList<int> RatingRangesCount
        {
            get => this.ratingRangesCount;

            set
            {
                this.ratingRangesCount = value;

                this.RatingsCount = this.ratingRangesCount.Sum();

                if (this.RatingsCount > 0)
                {
                    this.RatingRangesPercentage = new List<double>();

                    foreach (var count in this.ratingRangesCount)
                    {
                        this.RatingRangesPercentage.Add(count * 100 / this.RatingsCount);
                    }
                }
            }
        }

        public int RatingsCount { get; private set; }

        public IList<double> RatingRangesPercentage
        {
            get;

            private set;
        }

        public double AverageRating { get; set; }

        public decimal? HourWage { get; set; }

        public bool HasUserReviewedTeacher { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Teacher, TeacherDetails>()
                .ForMember(
                    x => x.AverageRating,
                    opt => opt.MapFrom(src => src.Reviews.Count != 0 ? Math.Round(src.Reviews.Average(x => x.Rating), 2) : 0))
                .ForMember(
                    x => x.RatingRangesCount,
                    opt => opt.MapFrom(src =>
                        new int[]
                        {
                            GetReviewsTypeCount(src.Reviews, 1),
                            GetReviewsTypeCount(src.Reviews, 2),
                            GetReviewsTypeCount(src.Reviews, 3),
                            GetReviewsTypeCount(src.Reviews, 4),
                            GetReviewsTypeCount(src.Reviews, 5),
                        }));
        }

        private static int GetReviewsTypeCount(ICollection<Review> reviews, int type)
        {
            return reviews.Select(x => x.Rating).Where(x => x == type).Count();
        }
    }
}
