namespace StudentsHelper.Web.ViewModels.StudentFavouriteTeacher
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentFavouriteTeacherViewModel : IMapFrom<Teacher>
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserPicturePath { get; set; }
    }
}
