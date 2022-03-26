namespace StudentsHelper.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class Teacher : BaseDeletableModel<string>
    {
        public Teacher()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Subjects = new HashSet<SchoolSubject>();
            this.Reviews = new HashSet<Review>();
            this.ReceivedPayments = new HashSet<StudentTransaction>();
            this.Consultations = new HashSet<Consultation>();
            this.IsValidated = false;
            this.IsRejected = false;
        }

        [Column(TypeName = "DECIMAL(5, 2)")]
        public decimal? HourWage { get; set; }

        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string QualificationDocumentPath { get; set; }

        [Required]
        public bool IsValidated { get; set; }

        [Required]
        public bool IsRejected { get; set; }

        public ICollection<SchoolSubject> Subjects { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<StudentTransaction> ReceivedPayments { get; set; }

        public ICollection<Consultation> Consultations { get; set; }

        [Required]
        public string ExpressConnectedAccountId { get; set; }

        [ForeignKey(nameof(School))]
        public int SchoolId { get; set; }

        public School School { get; set; }
    }
}
