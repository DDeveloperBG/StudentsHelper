namespace StudentsHelper.Services.Payments.Models
{
    public class CheckoutSessionData
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public decimal AmountAfterTax { get; set; }
    }
}
