namespace StudentsHelper.Web.ViewModels.SchoolSubjects
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class SchoolSubjectViewModel : IMapFrom<SchoolSubject>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconPath { get; set; }
    }
}
