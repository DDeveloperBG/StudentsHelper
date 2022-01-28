namespace StudentsHelper.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class Consultation : BaseDeletableModel<string>
    {
        public Consultation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [NotMapped]
        public TimeSpan Duration => this.EndTime - this.StartTime;

        [NotMapped]
        public decimal FullPrice => Math.Round(this.HourWage / 60 * (int)this.Duration.TotalMinutes, 2);

        [Required]
        public string Reason { get; set; }

        [Column(TypeName = "DECIMAL(5, 2)")]
        public decimal HourWage { get; set; }

        // Meeting
        [Required]
        [ForeignKey(nameof(Meeting))]
        public string MeetingId { get; set; }

        public Meeting Meeting { get; set; }

        // School Subject
        [ForeignKey(nameof(SchoolSubject))]
        public int SchoolSubjectId { get; set; }

        public SchoolSubject SchoolSubject { get; set; }

        // Student
        [Required]
        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public Student Student { get; set; }

        // Teacher
        [ForeignKey(nameof(Teacher))]
        public string TeacherId { get; set; }

        public Teacher Teacher { get; set; }
    }
}
