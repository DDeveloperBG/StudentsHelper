namespace StudentsHelper.Services.Data.Tests
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class GetAllReviewsForTeacherTestObject
         : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string StudentApplicationUserEmail { get; set; }

        public string StudentApplicationUserName { get; set; }

        public string StudentApplicationUserPicturePath { get; set; }

        public byte Rating { get; set; }

        public string Comment { get; set; }
    }
}
