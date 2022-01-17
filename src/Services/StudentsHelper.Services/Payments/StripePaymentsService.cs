namespace StudentsHelper.Services.Payments
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using Stripe;
    using Stripe.Checkout;
    using StudentsHelper.Data.Common.Repositories;
    using StudentsHelper.Data.Models;
    using StudentsHelper.Services.Payments.Models;

    public class StripePaymentsService : IPaymentsService
    {
        private readonly IStripeClient client;
        private readonly IOptions<StripeOptions> options;
        private readonly IRepository<StudentTransaction> studentsTransactionsRepository;

        public StripePaymentsService(
            IOptions<StripeOptions> options,
            IRepository<StudentTransaction> studentsTransactionsRepository)
        {
            this.options = options;
            this.client = new StripeClient(this.options.Value.SecretKey);
            this.studentsTransactionsRepository = studentsTransactionsRepository;
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

            await this.studentsTransactionsRepository.AddAsync(new StudentTransaction
            {
                StudentId = studentId,
                Amount = amount,
                SessionId = session.Id,
            });
            await this.studentsTransactionsRepository.SaveChangesAsync();

            return session.Url;
        }

        public int GetStudentBalance(string studentId)
        {
            return this.GetAllCompleted()
                .Where(x => x.StudentId == studentId)
                .Sum(x => x.Amount);
        }

        public Task MarkPaymentAsCompletedAsync(string sessionId)
        {
            var transaction = this.studentsTransactionsRepository
                .All()
                .Where(x => x.SessionId == sessionId)
                .SingleOrDefault();

            transaction.IsCompleted = true;

            return this.studentsTransactionsRepository.SaveChangesAsync();
        }

        private IQueryable<StudentTransaction> GetAllCompleted()
        {
            return this.studentsTransactionsRepository
                .All()
                .Where(x => x.IsCompleted);
        }
    }
}
