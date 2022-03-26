namespace StudentsHelper.Web.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using StudentsHelper.Common;
    using StudentsHelper.Common.Attributes;

    public class UserProfileInfoModel
    {
        [DataType(DataType.Upload)]
        [MaxFileSize(ValidationConstants.PictureValidSize)]
        [QualificationDocumentAllowedExtensionsAttribute]
        public IFormFile ProfilePicture { get; set; }

        [Display(Name = "Име")]
        [StringLength(
            ValidationConstants.NameMaxLength,
            MinimumLength = ValidationConstants.NameMinLength,
            ErrorMessage = "Името може да бъде най - малко {2} и максимум {1} символа дълго.")]
        [Required(ErrorMessage = ValidationConstants.RequiredError)]
        public string Name { get; set; }

        [Phone]
        [Display(Name = "Телефонен номер")]
        public string PhoneNumber { get; set; }
    }
}
