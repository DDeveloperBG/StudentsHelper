namespace StudentsHelper.Services.Auth
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class TeacherInputModel
    {
        [Required]
        public int SchoolId { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Документ за квалификация")]
        public IFormFile QualificationDocument { get; set; }

        public string QualificationDocumentPath { get; set; }
    }
}
