namespace StudentsHelper.Data.Models
{
    using System;

    using StudentsHelper.Data.Common.Models;

    public class Meeting : BaseModel<string>
    {
        public Meeting()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int DurationInMinutes { get; set; }

        public DateTime? TeacherLastActivity { get; set; }

        public DateTime? StudentLastActivity { get; set; }

        public Consultation Consultation { get; set; }
    }
}
