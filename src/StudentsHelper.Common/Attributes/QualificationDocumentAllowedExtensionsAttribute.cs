namespace StudentsHelper.Common.Attributes
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
