namespace StudentsHelper.Services.Payments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Web.ViewModels.Payments;

    public class MontlyPaymentsService : IMontlyPaymentsService
    {
        private readonly IPaymentsService paymentsService;
        private readonly ITeachersService teachersService;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public MontlyPaymentsService(
            IPaymentsService paymentsService,
            ITeachersService teachersService,
            IStudentsTransactionsService studentsTransactionsService)
        {
            this.paymentsService = paymentsService;
            this.teachersService = teachersService;
            this.studentsTransactionsService = studentsTransactionsService;
        }

        public async Task PayMontlySalariesAsync()
        {
            var teachers = this.teachersService
                .GetAllAsTracked<TeachersAndMonthTransactions>()
                .Where(x => x.CurrentMonthTransactions.Any());

            var allPaidTransactions = new List<StudentTransaction>();
            decimal websiteSalary = 0;
            foreach (var teacher in teachers)
            {
                var monthSalary = Math.Abs(teacher.MonthSalary);
                var websitePair = monthSalary * GlobalConstants.WebsiteMonthPercentageTax;
                websiteSalary += websitePair;
                var teacherPair = monthSalary - websitePair;
                await this.paymentsService.PayConnectedAccountAsync(teacher.TeacherConnectedAccountId, teacherPair);

                allPaidTransactions.AddRange(teacher.CurrentMonthTransactions);
            }

            await this.studentsTransactionsService.SetAsPaidTransactionsAsync(allPaidTransactions);
            await this.paymentsService.PayToWebsiteAsync(websiteSalary);
        }
    }
}
