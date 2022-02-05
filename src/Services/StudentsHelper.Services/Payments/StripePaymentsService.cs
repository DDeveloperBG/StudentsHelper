namespace StudentsHelper.Services.Payments
{
    using System;
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
            StripeConfiguration.ApiKey = this.options.Value.SecretKey;
            this.client = new StripeClient(this.options.Value.SecretKey);
            this.studentsTransactionsService = studentsTransactionsService;
        }

        public async Task<string> CreateCheckoutSessionAsync(string studentId, string studentEmail, int amount)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{this.options.Value.Domain}Identity/Balance?result=success",
                CancelUrl = $"{this.options.Value.Domain}Identity/Balance?result=canceled",
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

            var amountAfterTax = this.AmountAfterTaxes(amount);
            await this.studentsTransactionsService.AddStudentTransaction(studentId, amountAfterTax, session.Id);

            return session.Url;
        }

        public async Task<string> CreateTeacherExpressConnectedAccountAsync(string email)
        {
            var options = new AccountCreateOptions
            {
                Type = "express",
                Email = email,
                DefaultCurrency = "BGN",
                Capabilities = new AccountCapabilitiesOptions
                {
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true,
                    },
                },
                BusinessProfile = new AccountBusinessProfileOptions
                {
                    Mcc = "8299",

                    // It causes problems for localhost
                    // Url = $"{this.options.Value.Domain}/Teachers/Details?{nameof(teacherId)}={teacherId}",
                },
            };

            var service = new AccountService();
            var account = await service.CreateAsync(options);
            return account.Id;
        }

        public async Task<bool> IsTeacherExpressConnectedAccountConfirmedAsync(string accountId)
        {
            var service = new AccountService();

            var account = await service.GetAsync(accountId);
            return account.DetailsSubmitted && account.Capabilities.Transfers == "active";
        }

        public async Task<string> CreateAccountLinkAsync(string accountId)
        {
            var options = new AccountLinkCreateOptions
            {
                Account = accountId,
                RefreshUrl = this.options.Value.Domain,
                ReturnUrl = this.options.Value.Domain,
                Type = "account_onboarding",
            };
            var service = new AccountLinkService();
            var accountLink = await service.CreateAsync(options);
            return accountLink.Url;
        }

        public async Task PayConnectedAccountAsync(string accountId, decimal amount)
        {
            var options = new TransferCreateOptions
            {
                Amount = (long)(Math.Round(amount, 2) * 100),
                Currency = "BGN",
                Destination = accountId,
                Description = "Месечно възнаграждение от StudentsHelper",
            };

            var service = new TransferService();
            await service.CreateAsync(options);
        }

        public Task PayToWebsiteAsync(decimal amount)
        {
            var options = new PayoutCreateOptions
            {
                Amount = (long)(Math.Round(amount, 2) * 100),
                Currency = "BGN",
            };
            var service = new PayoutService();
            return service.CreateAsync(options);
        }

        // Important!
        private decimal AmountAfterTaxes(decimal amount)
        {
            const decimal taxPercentage = 2.9M / 100; // 2.9%
            const decimal taxFixedSum = 0.50M; // 0.50 BGN

            return Math.Round(amount - (amount * taxPercentage) - taxFixedSum, 2);
        }
    }
}
