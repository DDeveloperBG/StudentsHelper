namespace StudentsHelper.Services.Auth
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using StudentsHelper.Common;

    public class TeacherInputModel
    {
        [Display(Name = "Училище")]
        [Range(1, int.MaxValue, ErrorMessage = ValidationConstants.RequiredError)]
        public int SchoolId { get; set; }

        [DataType(DataType.Upload)]
        [QualificationDocumentAllowedExtensions]
        [Display(Name = "Документ за квалификация")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [MaxFileSize(ValidationConstants.QualificationDocumentValidSize)]
        public IFormFile QualificationDocument { get; set; }
    }
}
