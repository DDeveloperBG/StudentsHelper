namespace StudentsHelper.Web.Infrastructure.Filters
{
    using System;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class GuestFilterAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.Redirect("/Home/Index");
            }
        }
    }
}
