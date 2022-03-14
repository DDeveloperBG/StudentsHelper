namespace StudentsHelper.Services.Data.Tests
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class GetAllTestModel : IMapFrom<SchoolSubject>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconPath { get; set; }
    }
}
