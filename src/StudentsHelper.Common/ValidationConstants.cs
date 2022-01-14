namespace StudentsHelper.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static class ValidationConstants
    {
        public const string RequiredError = "Полето {0} е задължително.";

        public const int PictureValidSize = 25 * 1024 * 1024; // 50 Megabytes

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
