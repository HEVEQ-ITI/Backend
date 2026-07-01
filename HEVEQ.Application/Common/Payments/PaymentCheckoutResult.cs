namespace HEVEQ.Application.Common.Payments
{
    public class PaymentCheckoutResult
    {
        public string PaymentProvider { get; set; } = "Stripe";
        public string CheckoutUrl { get; set; } = string.Empty;
        public string Status { get; set; } = "CheckoutCreated";
        public string? PaymentGatewayReference { get; set; }
    }
}