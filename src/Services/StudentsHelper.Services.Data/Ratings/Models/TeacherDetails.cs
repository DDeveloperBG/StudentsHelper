namespace StudentsHelper.Services.Data.Ratings.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class TeacherDetails
    {
        private IList<int> ratingRangesCount;

        public string Id { get; set; }

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
    }
}
