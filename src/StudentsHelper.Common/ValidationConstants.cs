namespace StudentsHelper.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationConstants
    {
        // User Auth Constraints
        public const int NameMinLength = 5;

        public const int NameMaxLength = 32;

        public const int EmailMinLength = 6;

        public const int EmailMaxLength = 50;

        public const int PasswordMinLength = 6;

        public const int PasswordMaxLength = 50;

        // Other Constraints
        public const string RequiredError = "Полето {0} е задължително.";

        public const int PictureValidSize = 25 * 1024 * 1024; // 50 Megabytes

        public const int MinDepositAmount = 5;

        public const int MaxDepositAmount = 1000;

        public const int MinTeacherWage = 1;

        public const int MaxTeacherWage = 300;

        public static readonly Dictionary<string, string> ValidExteinsionsForPictureToMime
            = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".jpe", "image/jpeg" },
                { ".jif", "image/jpeg" },
                { ".jfif", "image/jpeg" },
                { ".jfi", "image/jpeg" },
                { ".png", "image/png" },
                { ".tiff", "image/tiff" },
                { ".tif", "image/tiff" },
                { ".bmp", "image/bmp" },
            };

        public static readonly string[] ValidExteinsionsForPicture
            = ValidExteinsionsForPictureToMime.Keys.ToArray();
    }
}
