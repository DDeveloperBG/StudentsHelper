namespace StudentsHelper.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class ReviewInputModel
    {
        [Required]
        public string TeacherId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Оценката е задължително поле.")]
        public byte Rating { get; set; }

        [MinLength(5, ErrorMessage = "Коментара може да е минимум {1} символа.")]
        [MaxLength(500, ErrorMessage = "Коментара може да е максимум {1} символа.")]
        public string Comment { get; set; } // Not Required
    }
}
