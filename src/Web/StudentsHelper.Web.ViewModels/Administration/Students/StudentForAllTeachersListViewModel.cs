namespace StudentsHelper.Web.ViewModels.Administration.Students
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class StudentForAllTeachersListViewModel : IMapFrom<Student>
    {
        public string Id { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string ApplicationUserPicturePath { get; set; }
    }
}
