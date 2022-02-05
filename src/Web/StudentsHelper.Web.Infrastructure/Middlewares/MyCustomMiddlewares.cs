namespace StudentsHelper.Web.Infrastructure.Middlewares
{
    using Microsoft.AspNetCore.Builder;

    public static class MyCustomMiddlewares
    {
        public static IApplicationBuilder UseUpdateUserActivityMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UpdateUserActivityMiddleware>();
        }

        public static IApplicationBuilder UseSetTeacherConnectedAccountMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetTeacherConnectedAccountMiddleware>();
        }
    }
}
