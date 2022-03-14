namespace StudentsHelper.Services.Data.Tests
{
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class GetOneTestObject : IMapFrom<Teacher>
    {
        public string Id { get; set; }

        public string ApplicationUserUserName { get; set; }

        public decimal HourWage { get; set; }

        public bool IsValidated { get; set; }

        public bool IsRejected { get; set; }
    }
}
