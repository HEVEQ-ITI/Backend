namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class MarketplaceOrderPaymentCheckoutResponseDto
    {
        public Guid MarketplaceOrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentProvider { get; set; } = "Stripe";
        public string CheckoutUrl { get; set; } = string.Empty;
        public string Status { get; set; } = "CheckoutCreated";
        public string PaymentGatewayReference { get; set; } = string.Empty;
    }
}