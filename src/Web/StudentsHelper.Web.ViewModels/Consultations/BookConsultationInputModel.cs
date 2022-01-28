namespace StudentsHelper.Web.ViewModels.Consultations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BookConsultationInputModel
    {
        [Required]
        [Display(Name = "Ден и час:")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Duration)]
        public TimeSpan Duration { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете цел на консултацията.")]
        [StringLength(500, ErrorMessage = "Текста трябва да е поне {2} символа и макс {1}.", MinimumLength = 20)]
        public string Reason { get; set; }

        [Required]
        public string TeacherId { get; set; }
    }
}
