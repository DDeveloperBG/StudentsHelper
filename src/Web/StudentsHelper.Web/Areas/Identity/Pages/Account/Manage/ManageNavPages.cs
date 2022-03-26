namespace StudentsHelper.Web.Areas.Identity.Pages.Account.Manage
{
    using System;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class ManageNavPages
    {
        public static string Index => nameof(Index);

        public static string Balance => nameof(Balance);

        public static string TeacherSchool => nameof(TeacherSchool);

        public static string TeacherDescription => nameof(TeacherDescription);

        public static string Email => nameof(Email);

        public static string ChangePassword => nameof(ChangePassword);

        public static string DownloadPersonalData => nameof(DownloadPersonalData);

        public static string DeletePersonalData => nameof(DeletePersonalData);

        public static string ExternalLogins => nameof(ExternalLogins);

        public static string PersonalData => nameof(PersonalData);

        public static string TwoFactorAuthentication => nameof(TwoFactorAuthentication);

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string BalanceNavClass(ViewContext viewContext) => PageNavClass(viewContext, Balance);

        public static string TeacherSchoolNavClass(ViewContext viewContext) => PageNavClass(viewContext, TeacherSchool);

        public static string TeacherDescriptionNavClass(ViewContext viewContext) => PageNavClass(viewContext, TeacherDescription);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
