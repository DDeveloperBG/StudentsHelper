namespace StudentsHelper.Web.Infrastructure.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Payments;

    public class SetTeacherConnectedAccountMiddleware
    {
        private readonly RequestDelegate next;

        public SetTeacherConnectedAccountMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IPaymentsService paymentsService,
            ITeachersService teachersService,
            UserManager<ApplicationUser> userManager)
        {
            if (context.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                var teacherEmail = context.User.Identity.Name;
                GlobalVariables.TeachersConnectedAccountStatus.TryGetValue(teacherEmail, out bool status);

                if (!status)
                {
                    var user = await userManager.GetUserAsync(context.User);
                    var teacherId = teachersService.GetId(user.Id);
                    var accountId = teachersService.GetExpressConnectedAccountId(teacherId);

                    var isConfirmed = await paymentsService.IsTeacherExpressConnectedAccountConfirmedAsync(accountId);
                    if (!isConfirmed)
                    {
                        var accountLink = await paymentsService.CreateAccountLinkAsync(accountId);
                        context.Response.Redirect(accountLink);
                        return;
                    }

                    GlobalVariables.TeachersConnectedAccountStatus[teacherEmail] = true;
                }
            }

            await this.next(context);
        }
    }
}
