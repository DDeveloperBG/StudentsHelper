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
            this.PaymentDate = DateTime.UtcNow;
            this.IsCompleted = false; // I know it would be false by default but I want it to be made clear
        }

        public int Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public bool IsCompleted { get; set; }

        public string SessionId { get; set; }

        [Required]
        [ForeignKey(nameof(Student))]
        public string StudentId { get; set; }

        public Student Student { get; set; }
    }
}
