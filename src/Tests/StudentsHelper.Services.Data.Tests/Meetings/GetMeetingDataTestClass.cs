namespace StudentsHelper.Services.Data.Tests
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class GetMeetingDataTestClass : IMapFrom<Meeting>
    {
        public string Id { get; set; }
    }
}
