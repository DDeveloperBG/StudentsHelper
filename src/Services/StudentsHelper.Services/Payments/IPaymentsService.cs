namespace StudentsHelper.Services.Payments
{
    using System.Threading.Tasks;

    public interface IPaymentsService
    {
        Task<string> CreateCheckoutSession(string studentId, string studentEmail, int amaount);

        Task MarkPaymentAsCompletedAsync(string sessionId);

        int GetStudentBalance(string studentId);
    }
}
