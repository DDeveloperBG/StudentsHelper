namespace StudentsHelper.Services.Auth
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            this.maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (file.Length > this.maxFileSize)
                {
                    return new ValidationResult(this.GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Максималната големина на файла е {this.maxFileSize} байта.";
        }
    }
}
