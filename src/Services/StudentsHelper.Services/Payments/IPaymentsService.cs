namespace StudentsHelper.Services.Payments
{
    using System.Threading.Tasks;

    public interface IPaymentsService
    {
        Task<string> CreateCheckoutSessionAsync(string studentId, string studentEmail, int amaount);

        Task<string> CreateTeacherExpressConnectedAccountAsync(string email);

        Task<string> CreateAccountLinkAsync(string accountId);

        Task<bool> IsTeacherExpressConnectedAccountConfirmedAsync(string accountId);

        Task PayConnectedAccountAsync(string accountId, decimal amount);

        Task PayToWebsiteAsync(decimal amount);
    }
}
