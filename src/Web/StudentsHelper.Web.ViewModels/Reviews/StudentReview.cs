namespace StudentsHelper.Web.ViewModels.Reviews
{
    using System;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentReview : IMapFrom<Review>
    {
        public int Id { get; set; }

        public string StudentApplicationUserEmail { get; set; }

        public string StudentApplicationUserName { get; set; }

        public string StudentApplicationUserPicturePath { get; set; }

        public byte Rating { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Comment { get; set; }
    }
}
