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

        [Display(Name = "Документ за квалификация")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [DataType(DataType.Upload)]
        [MaxFileSize(50 * 1024 * 1024)] // 50 Megabytes
        [AllowedExtensions(new string[] { ".pdf", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".png", ".tiff", ".tif", ".doc" })]
        public IFormFile QualificationDocument { get; set; }
    }
}
