﻿namespace StudentsHelper.Services.Payments.Models
{
    public class StripeOptions
    {
        public string PublishableKey { get; set; }

        public string SecretKey { get; set; }

        public string WebhookSecret { get; set; }

        public string Domain { get; set; }
    }
}
