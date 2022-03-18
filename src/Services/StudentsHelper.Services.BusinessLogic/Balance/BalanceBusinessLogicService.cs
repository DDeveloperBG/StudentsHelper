namespace StudentsHelper.Services.BusinessLogic.Balance
{
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Services.Data.Students;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Payments;
    using StudentsHelper.Web.ViewModels.Balance;

    public class BalanceBusinessLogicService : IBalanceBusinessLogicService
    {
        private readonly IPaymentsService paymentsService;
        private readonly IStudentsService studentsService;
        private readonly ITeachersService teachersService;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public BalanceBusinessLogicService(
            IPaymentsService paymentsService,
            IStudentsService studentsService,
            ITeachersService teachersService,
            IStudentsTransactionsService studentsTransactionsService)
        {
            this.paymentsService = paymentsService;
            this.studentsService = studentsService;
            this.teachersService = teachersService;
            this.studentsTransactionsService = studentsTransactionsService;
        }

        public BalanceViewModel GetIndexPageViewModel(string userId, bool userIsStudent, bool userIsTeacher)
        {
            var viewModel = new BalanceViewModel();

            if (userIsStudent)
            {
                string studentId = this.studentsService.GetId(userId);
                viewModel.BalanceAmount = this.studentsTransactionsService.GetStudentBalance(studentId);
                viewModel.TransactionsInfo = this.studentsTransactionsService.GetStudentTransactions<TransactionViewModel>(studentId);
            }
            else if (userIsTeacher)
            {
                string teacherId = this.teachersService.GetId(userId);
                viewModel.TeacherHourWage = this.teachersService.GetHourWage(teacherId);
                viewModel.BalanceAmount = this.studentsTransactionsService.GetTeacherBalance(teacherId);
                viewModel.TransactionsInfo = this.studentsTransactionsService.GetTeacherTransactions<TransactionViewModel>(teacherId);
            }

            viewModel.TransactionsInfo = viewModel.TransactionsInfo.OrderByDescending(x => x.PaymentDate);

            return viewModel;
        }

        /// <summary>
        /// Created checkout session and save info into database.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="userEmail">User email.</param>
        /// <param name="amount">Amount to be payed by student.</param>
        /// <returns>Returns created checkout session url.</returns>
        public async Task<string> StudentDepositAsync(string userId, string userEmail, int amount)
        {
            string studentId = this.studentsService.GetId(userId);

            var checkoutSessionData = await this
                .paymentsService
                .CreateCheckoutSessionAsync(
                    userEmail,
                    amount);

            await this
                .studentsTransactionsService
                .AddStudentTransactionAsync(
                    studentId,
                    checkoutSessionData.AmountAfterTax,
                    checkoutSessionData.Id);

            return checkoutSessionData.Url;
        }

        /// <summary>
        /// Updates teacher hourly wage.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <param name="newWage">New teacher hourly wage.</param>
        /// <returns></returns>
        public async Task SetTeacherWageAsync(string userId, decimal newWage)
        {
            string teacherId = this.teachersService.GetId(userId);

            await this
                .teachersService
                .ChangeTeacherHourWageAsync(
                    teacherId,
                    newWage);
        }
    }
}
