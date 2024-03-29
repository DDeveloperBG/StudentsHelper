﻿namespace StudentsHelper.Services.BusinessLogic.MontlyPayments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsHelper.Common;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Data.Teachers;
    using StudentsHelper.Services.Payments;
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
                .GetAllValidatedMappedAndTracked<TeachersAndMonthTransactions>()
                .Where(x => x.CurrentMonthTransactions.Any());

            var allPaidTransactions = new List<StudentTransaction>();
            decimal websiteSalary = 0;
            foreach (var teacher in teachers)
            {
                var monthSalary = Math.Abs(teacher.MonthSalary);
                var websitePair = monthSalary * GlobalConstants.WebsiteMonthPercentageTax;
                var teacherPair = monthSalary - websitePair;
                if (teacherPair > 1)
                {
                    websiteSalary += websitePair;
                    await this.paymentsService.PayConnectedAccountAsync(teacher.TeacherConnectedAccountId, teacherPair);
                    allPaidTransactions.AddRange(teacher.CurrentMonthTransactions);
                }
            }

            if (websiteSalary < 1)
            {
                return;
            }

            await this.studentsTransactionsService.SetAsPaidTransactionsAsync(allPaidTransactions);
            await this.paymentsService.PayToWebsiteAsync(websiteSalary);
        }
    }
}
