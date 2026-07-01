namespace HEVEQ.Infrastructure.Payments.Stripe
{
    public class StripePaymentOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string PublishableKey { get; set; } = string.Empty;
        public string WebhookSecret { get; set; } = string.Empty;
        public bool UseSimulatedCheckout { get; set; } = true;

        public string SimulatedCheckoutBaseUrl { get; set; } = "http://localhost:4200/payment/checkout-simulated";
        public string DefaultSuccessUrl { get; set; } = "http://localhost:4200/payment/success?session_id={CHECKOUT_SESSION_ID}";
        public string DefaultCancelUrl { get; set; } = "http://localhost:4200/payment/cancel";
    }
}