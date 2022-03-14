namespace StudentsHelper.Services.Data.Tests
{
    using System;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class GetConsultationsTestObject : IMapFrom<Consultation>
    {
        public string Id { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string TeacherId { get; set; }

        public string StudentId { get; set; }
    }
}
