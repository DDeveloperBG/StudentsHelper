namespace StudentsHelper.Services.Data.Ratings.Models
{
    public class TeacherWithRating
    {
        public string Id { get; set; }

        public decimal HourWage { get; set; }

        public string ApplicationUserEmail { get; set; }

        public bool IsActive { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserPicturePath { get; set; }

        public double AverageRating { get; set; }
    }
}
