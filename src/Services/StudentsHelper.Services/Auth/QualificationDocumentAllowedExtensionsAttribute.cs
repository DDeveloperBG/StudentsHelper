namespace StudentsHelper.Services.Auth
{
    using StudentsHelper.Common;

    public class QualificationDocumentAllowedExtensionsAttribute : AllowedExtensionsAttribute
    {
        public QualificationDocumentAllowedExtensionsAttribute()
            : base(ValidationConstants.ValidExtensionsForPicture)
        {
        }
    }
}
