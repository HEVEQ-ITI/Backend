using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketplaceOrderPayment
{
    public sealed record ConfirmMarketplaceOrderPaymentCommand(Guid MarketplaceOrderId, string? PaymentGatewayReference) : IRequest<MarketplaceOrderPaymentConfirmResponseDto>;
}