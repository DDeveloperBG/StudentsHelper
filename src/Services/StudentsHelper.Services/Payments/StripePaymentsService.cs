namespace StudentsHelper.Services.Payments
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using Stripe;
    using Stripe.Checkout;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Payments.Models;

    public class StripePaymentsService : IPaymentsService
    {
        private readonly IStripeClient client;
        private readonly IOptions<StripeOptions> options;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public StripePaymentsService(
            IOptions<StripeOptions> options,
            IStudentsTransactionsService studentsTransactionsService)
        {
            this.options = options;
            this.client = new StripeClient(this.options.Value.SecretKey);
            this.studentsTransactionsService = studentsTransactionsService;
        }

        public async Task<string> CreateCheckoutSession(string studentId, string studentEmail, int amount)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{this.options.Value.Domain}Identity/Account/Manage/Balance?result=success",
                CancelUrl = $"{this.options.Value.Domain}Identity/Account/Manage/Balance?result=canceled",
                Mode = "payment",
                CustomerEmail = studentEmail,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = amount * 2,
                        Price = $"price_1KIVcVEML6qzx0YHyC20cJVE",
                    },
                },
            };

            var service = new SessionService(this.client);
            var session = await service.CreateAsync(options);

            await this.studentsTransactionsService.AddStudentTransaction(studentId, amount, session.Id);

            return session.Url;
        }
    }
}
