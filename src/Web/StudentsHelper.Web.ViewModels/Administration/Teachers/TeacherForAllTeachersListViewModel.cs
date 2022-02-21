namespace StudentsHelper.Web.ViewModels.Administration.Teachers
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeacherForAllTeachersListViewModel : IMapFrom<Teacher>
    {
        public string Id { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserPicturePath { get; set; }

        public bool IsValidated { get; set; }

        public bool IsRejected { get; set; }
    }
}
