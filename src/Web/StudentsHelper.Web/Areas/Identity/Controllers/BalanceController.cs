namespace StudentsHelper.Web.Areas.Identity.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Payments;
    using StudentsHelper.Web.Controllers;
    using StudentsHelper.Web.Infrastructure.Alerts;
    using StudentsHelper.Web.ViewModels.Balance;

    [Authorize]
    [Area("Identity")]
    public class BalanceController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPaymentsService paymentsService;
        private readonly IStudentsService studentsService;
        private readonly ITeachersService teachersService;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public BalanceController(
            UserManager<ApplicationUser> userManager,
            IPaymentsService paymentsService,
            IStudentsService studentsService,
            ITeachersService teachersService,
            IStudentsTransactionsService studentsTransactionsService)
        {
            this.userManager = userManager;
            this.paymentsService = paymentsService;
            this.studentsService = studentsService;
            this.teachersService = teachersService;
            this.studentsTransactionsService = studentsTransactionsService;
        }

        public async Task<IActionResult> IndexAsync(string result = null)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var viewModel = new BalanceViewModel();

            if (this.User.IsInRole(GlobalConstants.StudentRoleName))
            {
                string studentId = this.studentsService.GetId(user.Id);
                viewModel.BalanceAmount = this.studentsTransactionsService.GetStudentBalance(studentId);
                viewModel.TransactionsInfo = this.studentsTransactionsService.GetStudentTransactions<TransactionViewModel>(studentId);
            }
            else if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                string teacherId = this.teachersService.GetId(user.Id);
                viewModel.TeacherHourWage = this.teachersService.GetHourWage(teacherId);
                viewModel.BalanceAmount = this.studentsTransactionsService.GetTeacherBalance(teacherId);
                viewModel.TransactionsInfo = this.studentsTransactionsService.GetTeacherTransactions<TransactionViewModel>(teacherId);
            }

            viewModel.TransactionsInfo = viewModel.TransactionsInfo.OrderByDescending(x => x.PaymentDate);

            IActionResult responce = this.View(viewModel);
            if (result != null)
            {
                this.HttpContext.Session.TryGetValue("returnUrl", out byte[] outputBytes);
                this.HttpContext.Session.Remove("returnUrl");

                if (outputBytes != null)
                {
                    var returnUrl = Encoding.UTF8.GetString(outputBytes);

                    if (returnUrl != null)
                    {
                        responce = this.Redirect(returnUrl);
                    }
                }
            }

            switch (result)
            {
                case "success": return responce.WithSuccess("Плащането бе успешно!");
                case "canceled": return responce.WithDanger("Плащането бе отказано!");
                default: return responce;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DepositAsync(DepositInputModel depositInput)
        {
            if (!this.ModelState.IsValid)
            {
                return (await this.IndexAsync()).WithDanger("Невалидни данни!");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            string studentId = this.studentsService.GetId(user.Id);
            string paymentUrl = await this
                .paymentsService
                .CreateCheckoutSessionAsync(
                    studentId,
                    user.Email,
                    depositInput.DepositRequestMoneyAmount);

            return this.Redirect(paymentUrl);
        }

        [HttpPost]
        public async Task<IActionResult> SetTeacherWageAsync(TeacherWageInputModel teacherWageInput)
        {
            if (!this.ModelState.IsValid)
            {
                return (await this.IndexAsync()).WithDanger("Невалидни данни!");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            string teacherId = this.teachersService.GetId(user.Id);
            await this.teachersService
                .ChangeTeacherHourWageAsync(
                    teacherId,
                    teacherWageInput.TeacherWage);

            return this.RedirectToAction("Index");
        }
    }
}
