namespace StudentsHelper.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;

    using StudentsHelper.Common;

    public class ContactInputModel
    {
        [Display(Name = "Заглавие")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Заглавието трябва да бъде поне {2} и максимум {1} символа дълго.")]
        public string Title { get; set; }

        [Display(Name = "Имейл")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        public string Email { get; set; }

        [Display(Name = "Съдържание")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        [StringLength(20000, MinimumLength = 12, ErrorMessage = "Съдържанието трябва да бъде поне {2} и максимум {1} символа дълго.")]
        public string Message { get; set; }
    }
}
