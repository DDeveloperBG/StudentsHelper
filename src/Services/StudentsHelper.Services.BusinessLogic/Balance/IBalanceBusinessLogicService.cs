namespace StudentsHelper.Services.BusinessLogic.Balance
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Balance;

    public interface IBalanceBusinessLogicService
    {
        BalanceViewModel GetIndexPageViewModel(string userId, bool userIsStudent, bool userIsTeacher);

        Task<string> StudentDepositAsync(string userId, string userEmail, int amount);

        Task SetTeacherWageAsync(string userId, decimal newWage);
    }
}
