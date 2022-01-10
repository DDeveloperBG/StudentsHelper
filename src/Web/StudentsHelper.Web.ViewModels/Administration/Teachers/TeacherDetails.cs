namespace StudentsHelper.Web.ViewModels.Administration.Teachers
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class TeacherDetails : IMapFrom<Teacher>
    {
        public string Id { get; set; }

        public string QualificationDocumentName { get; set; }

        public string ApplicationUserName { get; set; }

        public string ApplicationUserPicturePath { get; set; }
    }
}
