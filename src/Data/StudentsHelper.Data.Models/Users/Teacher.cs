namespace StudentsHelper.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using StudentsHelper.Data.Common.Models;

    public class Teacher : BaseDeletableModel<string>
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string QualificationDocumentPath { get; set; }

        public bool IsValidated { get; set; }

        [ForeignKey(nameof(Subject))]
        public int SubjectId { get; set; }

        public SchoolSubject Subject { get; set; }

        [ForeignKey(nameof(School))]
        public int SchoolId { get; set; }

        public School School { get; set; }
    }
}
