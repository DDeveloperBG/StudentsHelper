namespace StudentsHelper.Data
{
    using Microsoft.AspNetCore.Identity;

    public static class IdentityOptionsProvider
    {
        public static void GetIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            options.SignIn.RequireConfirmedAccount = false; // In production to have to be true
            options.SignIn.RequireConfirmedEmail = false; // In production to have to be true

            options.Lockout.MaxFailedAccessAttempts = 15;
            options.Lockout.DefaultLockoutTimeSpan = new System.TimeSpan(hours: 1, 0, 0);
        }
    }
}
