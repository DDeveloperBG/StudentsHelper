namespace StudentsHelper.Services.Auth
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class TeacherInputModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Полето {0} е задължително.")]
        [Display(Name = "Училище")]
        public int SchoolId { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително.")]
        [DataType(DataType.Upload)]
        [MaxFileSize(50 * 1024 * 1024)] // 50 Megabytes
        [AllowedExtensions(new string[] { ".pdf", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".jfi", ".png", ".tiff", ".tif", ".doc" })]
        [Display(Name = "Документ за квалификация")]
        public IFormFile QualificationDocument { get; set; }
    }
}
