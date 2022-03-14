namespace StudentsHelper.Services.Data.Tests
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class GetOneTestClass : IMapFrom<Student>
    {
        public string Id { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
