namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class MarketplaceOrderPaymentConfirmResponseDto
    {
        public Guid MarketplaceOrderId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string OrderStatusAr { get; set; } = string.Empty;
        public string EscrowStatus { get; set; } = string.Empty;
        public string EscrowStatusAr { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}