namespace StudentsHelper.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Stripe;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Payments.Models;

    [IgnoreAntiforgeryToken]
    public class PaymentsWebhookController : BaseController
    {
        private readonly string webhookSecret;
        private readonly IStudentsTransactionsService studentsTransactionsService;

        public PaymentsWebhookController(
            IOptions<StripeOptions> options,
            IStudentsTransactionsService studentsTransactionsService)
        {
            this.webhookSecret = options.Value.WebhookSecret;
            this.studentsTransactionsService = studentsTransactionsService;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> IndexAsync()
        {
            var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    this.Request.Headers["Stripe-Signature"],
                    this.webhookSecret);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    if (session?.PaymentStatus == "paid")
                    {
                        await this.studentsTransactionsService.MarkPaymentAsCompletedAsync(session.Id);
                    }
                }

                return this.Ok();
            }
            catch
            {
                return this.BadRequest();
            }
        }
    }
}
