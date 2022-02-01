namespace StudentsHelper.Web.Infrastructure
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using StudentsHelper.Common;

    public static class Middlewares
    {
        public static Task AddUserActivityAsync(HttpContext context, Func<Task> next)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                GlobalVariables.UsersActivityDictionary[context.User.Identity.Name] = DateTime.UtcNow;
            }

            if (context.Request.Query.ContainsKey("userStatusUpdate"))
            {
                return context.Response.WriteAsync("Status updated.");
            }

            return next();
        }
    }
}
