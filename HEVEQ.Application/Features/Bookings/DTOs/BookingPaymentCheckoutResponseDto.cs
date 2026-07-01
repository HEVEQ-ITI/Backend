namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class BookingPaymentCheckoutResponseDto
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentProvider { get; set; } = "Stripe";
        public string CheckoutUrl { get; set; } = string.Empty;
        public string Status { get; set; } = "CheckoutCreated";
        public string PaymentGatewayReference { get; set; } = string.Empty;
    }
}