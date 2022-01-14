using Stripe;

namespace StudentsHelper.Services.Payments
{
    public class StripePaymentsService : IPaymentsService
    {
        private readonly string apiKey;

        public StripePaymentsService(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void A()
        {
            var options = new RequestOptions
            {
                ApiKey = this.apiKey,
            };

            var service = new ChargeService();
            Charge charge = service.Get(
              "ch_3KHmMh2eZvKYlo2C0OUY2Huv",
              requestOptions: options);
        }
    }
}
