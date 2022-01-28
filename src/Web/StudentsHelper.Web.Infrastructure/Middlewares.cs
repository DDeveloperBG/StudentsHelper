namespace StudentsHelper.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using StudentsHelper.Common;

    public static class Middlewares
    {
        public static Task AddTeacherActivityAsync(HttpContext context, Func<Task> next)
        {
            if (context.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                GlobalVariables.TeachersActivityDictionary
                    .AddOrUpdate(context.User.Identity.Name, DateTime.UtcNow, (a, b) => { return b; });
            }

            return next();
        }
    }
}
