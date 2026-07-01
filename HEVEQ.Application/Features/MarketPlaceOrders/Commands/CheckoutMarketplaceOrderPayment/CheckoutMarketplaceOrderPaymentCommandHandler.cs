using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Payments;
using HEVEQ.Application.Features.MarketPlaceOrders.Common;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CheckoutMarketplaceOrderPayment
{
    public sealed class CheckoutMarketplaceOrderPaymentCommandHandler : IRequestHandler<CheckoutMarketplaceOrderPaymentCommand, MarketplaceOrderPaymentCheckoutResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPaymentCheckoutService _paymentCheckoutService;
        private readonly ICurrentUserService _currentUserService;

        public CheckoutMarketplaceOrderPaymentCommandHandler(IApplicationDbContext context, IPaymentCheckoutService paymentCheckoutService, ICurrentUserService currentUserService)
        {
            _context = context;
            _paymentCheckoutService = paymentCheckoutService;
            _currentUserService = currentUserService;
        }

        public async Task<MarketplaceOrderPaymentCheckoutResponseDto> Handle(CheckoutMarketplaceOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var buyerId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

            var order = await _context.MarketplaceOrders
                .FirstOrDefaultAsync(x => x.Id == request.MarketplaceOrderId, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("Marketplace order was not found.");

            if (order.BuyerId != buyerId)
                throw new UnauthorizedAccessException("Only the order buyer can pay for this order.");

            if (order.Status != MarketplaceOrderStatus.PendingPayment)
                throw new InvalidOperationException("Marketplace order is not ready for payment.");

            if (order.Amount <= 0)
                throw new InvalidOperationException("Marketplace order amount is invalid.");

            var orderNumber = OrderNumberFormatter.Generate(order.Id, order.CreatedAt);

            var checkout = await _paymentCheckoutService.CreateCheckoutAsync(
                new PaymentCheckoutCreateRequest
                {
                    ReferenceType = PaymentReferenceType.MarketplaceOrder,
                    ReferenceId = order.Id,
                    ReferenceNumber = orderNumber,
                    PayingUserId = buyerId,
                    Amount = order.Amount,
                    Currency = "EGP",
                    PaymentMethod = request.PaymentMethod,
                    SuccessUrl = request.SuccessUrl,
                    CancelUrl = request.CancelUrl
                },
                cancellationToken
            );

            return new MarketplaceOrderPaymentCheckoutResponseDto
            {
                MarketplaceOrderId = order.Id,
                OrderNumber = orderNumber,
                Amount = order.Amount,
                Currency = "EGP",
                PaymentProvider = checkout.PaymentProvider,
                CheckoutUrl = checkout.CheckoutUrl,
                Status = checkout.Status,
                PaymentGatewayReference = checkout.PaymentGatewayReference
            };
        }
    }
}