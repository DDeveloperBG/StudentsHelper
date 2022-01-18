namespace StudentsHelper.Web.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Payments;

    public class Balance : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPaymentsService paymentsService;
        private readonly IStudentsService studentsService;
        private readonly ITeachersService teachersService;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public Balance(
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

        [TempData]
        public string StatusMessage { get; set; }

        [TempData]
        public int BalanceAmount { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Пари за курсове - лв")]
            [Required(ErrorMessage = ValidationConstants.RequiredError)]
            [Range(
                ValidationConstants.MinDepositAmount,
                ValidationConstants.MaxDepositAmount,
                ErrorMessage = "Стойноста трябва да е поне {1} и не повече от {2} лв.")]
            public int DepositRequestMoneyAmount { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string result = null)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (this.User.IsInRole(GlobalConstants.StudentRoleName))
            {
                string studentId = this.studentsService.GetId(user.Id);
                this.BalanceAmount = this.studentsTransactionsService.GetStudentBalance(studentId);
            }
            else if (this.User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                string teacherId = this.teachersService.GetId(user.Id);
                this.BalanceAmount = this.studentsTransactionsService.GetTeacherBalance(teacherId);
            }

            switch (result)
            {
                case "success": this.StatusMessage = "Плащането бе успешно!"; break;
                case "canceled": this.StatusMessage = "Error: Плащането бе отказано."; break;
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                this.StatusMessage = "Error: Невалидни данни!";
                return await this.OnGetAsync();
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Не може да се зареди потребител с ID '{this.userManager.GetUserId(this.User)}'.");
            }

            string studentId = this.studentsService.GetId(user.Id);
            string paymentUrl = await this
                .paymentsService
                .CreateCheckoutSession(
                    studentId,
                    user.Email,
                    this.Input.DepositRequestMoneyAmount);

            return this.Redirect(paymentUrl);
        }
    }
}
