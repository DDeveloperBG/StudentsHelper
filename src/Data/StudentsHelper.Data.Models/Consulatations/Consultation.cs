namespace StudentsHelper.Data.Models.Consulatations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class Consultation : BaseModel<string>
    {
        public Consultation()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public TimeSpan Duration { get; set; }

        public decimal HourWage { get; set; }

        [Required]
        [ForeignKey(nameof(StudentTransaction))]
        public string StudentTransactionId { get; set; }

        public StudentTransaction StudentTransaction { get; set; }
    }
}
