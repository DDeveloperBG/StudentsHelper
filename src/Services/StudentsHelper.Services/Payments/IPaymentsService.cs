namespace StudentsHelper.Services.Payments
{
    using System.Threading.Tasks;

    using StudentsHelper.Services.Payments.Models;

    public interface IPaymentsService
    {
        Task<CheckoutSessionData> CreateCheckoutSessionAsync(string studentEmail, int amount);

        Task<string> CreateTeacherExpressConnectedAccountAsync(string email, string teacherId, bool isInProduction);

        Task<string> CreateAccountLinkAsync(string accountId);

        Task<bool> IsTeacherExpressConnectedAccountConfirmedAsync(string accountId);

        Task PayConnectedAccountAsync(string accountId, decimal amount);

        Task PayToWebsiteAsync(decimal amount);
    }
}
