namespace StudentsHelper.Web.ViewModels.Teachers
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeacherViewModel : IMapFrom<Teacher>
    {
        public string Id { get; set; }

        public string ApplicationUserName { get; set; }

        public string PicturePath { get; set; }
    }
}
