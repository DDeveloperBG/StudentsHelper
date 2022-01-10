namespace StudentsHelper.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationConstants
    {
        public const string RequiredError = "Полето {0} е задължително.";

        public const int QualificationDocumentValidSize = 50 * 1024 * 1024; // 50 Megabytes

        public static readonly Dictionary<string, string> ValidExteinsionsForQualificationDocumentToMime
            = new Dictionary<string, string>
            {
                { ".pdf", "application/pdf" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".jpe", "image/jpeg" },
                { ".jif", "image/jpeg" },
                { ".jfif", "image/jpeg" },
                { ".jfi", "image/jpeg" },
                { ".png", "image/png" },
                { ".tiff", "image/tiff" },
                { ".tif", "image/tiff" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
            };

        public static readonly string[] ValidExteinsionsForQualificationDocument
            = ValidExteinsionsForQualificationDocumentToMime.Keys.ToArray();
    }
}
