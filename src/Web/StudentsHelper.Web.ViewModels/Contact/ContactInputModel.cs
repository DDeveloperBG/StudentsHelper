namespace StudentsHelper.Web.ViewModels.Contact
{
    using System.ComponentModel.DataAnnotations;

    public class ContactInputModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете вашите имена")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете вашият email адрес")]
        [EmailAddress(ErrorMessage = "Моля въведете валиден email адрес")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете заглавие на съобщението")]
        [StringLength(100, ErrorMessage = "Заглавието трябва да е поне {2} и не повече от {1} символа.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете съдържание на съобщението")]
        [StringLength(10000, ErrorMessage = "Съобщението трябва да е поне {2} символа.", MinimumLength = 20)]
        public string Content { get; set; }
    }
}
