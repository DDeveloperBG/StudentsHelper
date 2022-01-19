namespace StudentsHelper.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.EntityFrameworkCore;

    using StudentsHelper.Data.Common.Models;

    [Index(nameof(SessionId))]
    public class StudentTransaction : BaseModel<string>
    {
        public StudentTransaction()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsCompleted = false; // I know it would be false by default but I want to make it clear
        }

        // If it is deposit it would be possitive, if it is payment to teacher it would have negative value
        [Column(TypeName = "DECIMAL(8, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public bool IsCompleted { get; set; }

        public string SessionId { get; set; }

        [Required]
        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public Student Student { get; set; }

        [ForeignKey(nameof(ToTeacher))]
        public string ToTeacherId { get; set; }

        public Teacher ToTeacher { get; set; }
    }
}
