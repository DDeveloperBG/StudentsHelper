namespace StudentsHelper.Web.ViewModels.Consultations
{
    using System;

    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Mapping;

    public class ConsultationViewModel : IMapFrom<Consultation>
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan Duration { get; set; }

        public decimal FullPrice { get; set; }

        public string MeetingId { get; set; }

        public string SchoolSubjectName { get; set; }
    }
}
