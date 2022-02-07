namespace StudentsHelper.Web.Infrastructure.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using StudentsHelper.Common;
    using StudentsHelper.Services.Time;

    public class UpdateUserActivityMiddleware
    {
        private readonly RequestDelegate next;

        public UpdateUserActivityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context, IDateTimeProvider dateTimeProvider)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                GlobalVariables.UsersActivityDictionary[context.User.Identity.Name] = dateTimeProvider.GetUtcNow();
            }

            if (context.Request.Query.ContainsKey("userStatusUpdate"))
            {
                return context.Response.WriteAsync("Status updated.");
            }

            return this.next(context);
        }
    }
}
