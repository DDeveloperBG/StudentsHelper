namespace StudentsHelper.Web.Areas.Identity.Controllers
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.BusinessLogic.Balance;
    using StudentsHelper.Web.Controllers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Balance;

    [Authorize]
    [Area("Identity")]
    public class BalanceController : BaseController
    {
        private readonly IBalanceBusinessLogicService balanceBusinessLogicService;

        public BalanceController(
            UserManager<ApplicationUser> userManager,
            IBalanceBusinessLogicService balanceBusinessLogicService)
            : base(userManager)
        {
            this.balanceBusinessLogicService = balanceBusinessLogicService;
        }

        public async Task<IActionResult> IndexAsync(string result = null)
        {
            var user = await this.GetCurrentUserDataAsync();
            if (user == null)
            {
                return this.NotFound();
            }

            bool userIsStudent = this.User.IsInRole(GlobalConstants.StudentRoleName);
            bool userIsTeacher = this.User.IsInRole(GlobalConstants.TeacherRoleName);

            var viewModel = this
                .balanceBusinessLogicService
                .GetIndexPageViewModel(
                    user.Id,
                    userIsStudent,
                    userIsTeacher);

            IActionResult responce = this.View(viewModel);
            if (result != null)
            {
                this.HttpContext.Session.TryGetValue(GlobalConstants.ReturnUrlSessionValueKey, out byte[] outputBytes);

                if (outputBytes != null)
                {
                    this.HttpContext.Session.Remove(GlobalConstants.ReturnUrlSessionValueKey);
                    var returnUrl = Encoding.UTF8.GetString(outputBytes);

                    if (returnUrl != null)
                    {
                        responce = this.Redirect(returnUrl);
                    }
                }
            }

            switch (result)
            {
                case GlobalConstants.PaymentStatusMessages.Success: return responce.WithSuccess(GlobalConstants.PaymentMessages.SuccessfulPaymentMessage);
                case GlobalConstants.PaymentStatusMessages.Canceled: return responce.WithDanger(GlobalConstants.PaymentMessages.FailedPaymentMessage);
                default: return responce;
            }
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.StudentRoleName)]
        public async Task<IActionResult> DepositAsync(DepositInputModel depositInput)
        {
            if (!this.ModelState.IsValid)
            {
                return (await this.IndexAsync()).WithDanger(ValidationConstants.GeneralError);
            }

            var user = await this.GetCurrentUserDataAsync();
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            string paymentUrl = await this
                .balanceBusinessLogicService
                .StudentDepositAsync(
                    user.Id,
                    user.Email,
                    depositInput.DepositRequestMoneyAmount);

            if (paymentUrl == null)
            {
                return (await this.IndexAsync()).WithDanger(ValidationConstants.GeneralError);
            }

            return this.Redirect(paymentUrl);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.TeacherRoleName)]
        public async Task<IActionResult> SetTeacherWageAsync(TeacherWageInputModel teacherWageInput)
        {
            if (!this.ModelState.IsValid)
            {
                return (await this.IndexAsync()).WithDanger(ValidationConstants.GeneralError);
            }

            var user = await this.GetCurrentUserDataAsync();
            if (user == null)
            {
                return this.NotFound(GlobalConstants.GeneralMessages.UserNotFoundMessage);
            }

            await this.balanceBusinessLogicService
                .SetTeacherWageAsync(
                    user.Id,
                    teacherWageInput.TeacherWage);

            return this.RedirectToAction("Index");
        }
    }
}
