namespace StudentsHelper.Web.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Stripe;
    using StudentsHelper.Services.Data.StudentTransactions;
    using StudentsHelper.Services.Payments.Models;
    using StudentsHelper.Services.Time;

    [IgnoreAntiforgeryToken]
    public class PaymentsWebhookController : BaseController
    {
        private readonly string webhookSecret;
        private readonly IStudentsTransactionsService studentsTransactionsService;
        private readonly IDateTimeProvider dateTimeProvider;

        // private readonly IWebHostEnvironment webHostEnvironment;
        public PaymentsWebhookController(
            IOptions<StripeOptions> options,
            IStudentsTransactionsService studentsTransactionsService,
            IDateTimeProvider dateTimeProvider)
        {
            this.webhookSecret = options.Value.WebhookSecret;
            this.studentsTransactionsService = studentsTransactionsService;
            this.dateTimeProvider = dateTimeProvider;
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

                // For now, I will still use stripe in test mode even in production for demo purposes!
                //  if (stripeEvent.Livemode != this.webHostEnvironment.IsProduction())
                //  {
                //      return this.BadRequest();
                //  }
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    if (session?.PaymentStatus == "paid")
                    {
                        await this.studentsTransactionsService.MarkPaymentAsCompletedAsync(session.Id, this.dateTimeProvider.GetUtcNow());
                    }
                }

                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(new { Message = "Webhook was sent but failed inside the app", ErrorMessage = e.Message });
            }
        }
    }
}
