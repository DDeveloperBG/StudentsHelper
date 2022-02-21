namespace StudentsHelper.Web.ViewModels.User
{
    public class UserProfilePictureViewModel
    {
        public string PicturePath { get; set; }

        public bool? IsActive { get; set; }

        public bool IsInTeacherRole { get; set; }

        public string TeacherId { get; set; }
    }
}
