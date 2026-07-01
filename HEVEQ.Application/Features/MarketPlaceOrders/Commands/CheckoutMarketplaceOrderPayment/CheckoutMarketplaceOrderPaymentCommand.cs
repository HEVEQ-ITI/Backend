using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CheckoutMarketplaceOrderPayment
{
    public sealed record CheckoutMarketplaceOrderPaymentCommand(Guid MarketplaceOrderId, string PaymentMethod, string? SuccessUrl, string? CancelUrl) : IRequest<MarketplaceOrderPaymentCheckoutResponseDto>;
}