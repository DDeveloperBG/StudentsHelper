namespace StudentsHelper.Services.Data.Subjects
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class SchoolSubjectPickViewModel : IMapFrom<SchoolSubject>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
