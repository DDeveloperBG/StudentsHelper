namespace StudentsHelper.Web.ViewModels.Balance
{
    using System.Collections.Generic;

    public class BalanceViewModel
    {
        public decimal BalanceAmount { get; set; }

        public decimal? TeacherHourWage { get; set; }

        public IEnumerable<TransactionViewModel> TransactionsInfo { get; set; } = new List<TransactionViewModel>();
    }
}
