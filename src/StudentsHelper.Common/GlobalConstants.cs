namespace StudentsHelper.Common
{
    using System.Text.Encodings.Web;

    public static class GlobalConstants
    {
        public const string SystemName = "StudentsHelper";

        public const string ContactEmail = "dakataebg@students.softuni.bg";

        public const string AdministratorRoleName = "administrator";

        public const string TeacherRoleName = "teacher";

        public const string StudentRoleName = "student";

        public const string SiteDescription = $"{SystemName} има за цел да помогне на учениците с техните домашни, уроци и т.н. чрез мигновени консултации по конкретна тема с учител, специалист в областта.";

        public const string DeletedUserUsername = "deleted_user";

        public const string NoProfilePicturePath = "/img/noProgilePicture.png";

        public const string ConfirmEmailTitle = "Потвърдете акаунта си";

        public const string ProfilePicturesFolder = "ProfilePictures";

        public const decimal WebsiteMonthPercentageTax = 0.05M; // 10%

        public static string GetEmailConfirmationMessage(string confirmUrl) => $"<h1>Моля потвърдете акаунта си, като <a href='{HtmlEncoder.Default.Encode(confirmUrl)}'>кликнете тук</a>.";
    }
}
