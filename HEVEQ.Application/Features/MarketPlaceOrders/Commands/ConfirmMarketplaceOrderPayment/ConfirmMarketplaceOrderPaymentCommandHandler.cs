using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Common.Payments;
using HEVEQ.Application.Features.Bookings.Helpers;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketplaceOrderPayment
{
    public sealed class ConfirmMarketplaceOrderPaymentCommandHandler : IRequestHandler<ConfirmMarketplaceOrderPaymentCommand, MarketplaceOrderPaymentConfirmResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly PaymentPlatformOptions _paymentOptions;
        private readonly ICurrentUserService _currentUserService;

        public ConfirmMarketplaceOrderPaymentCommandHandler(IApplicationDbContext context, IOptions<PaymentPlatformOptions> paymentOptions, ICurrentUserService currentUserService)
        {
            _context = context;
            _paymentOptions = paymentOptions.Value;
            _currentUserService = currentUserService;
        }

        public async Task<MarketplaceOrderPaymentConfirmResponseDto> Handle(ConfirmMarketplaceOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var buyerId = _currentUserService.UserId
                ?? throw new UnauthorizedAccessException("User is not authenticated.");

            var order = await _context.MarketplaceOrders
                .Include(x => x.EscrowRecords)
                .FirstOrDefaultAsync(x => x.Id == request.MarketplaceOrderId, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("Marketplace order was not found.");

            if (order.BuyerId != buyerId)
                throw new UnauthorizedAccessException("Only the order buyer can confirm payment for this order.");

            if (order.Status != MarketplaceOrderStatus.PendingPayment)
                throw new InvalidOperationException("Marketplace order is not ready for payment confirmation.");

            if (order.Amount <= 0)
                throw new InvalidOperationException("Marketplace order amount is invalid.");

            var existingEscrow = order.EscrowRecords
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefault();

            if (existingEscrow is not null)
                throw new InvalidOperationException("Escrow record already exists for this marketplace order.");

            var now = DateTime.Now;

            var commissionRate = _paymentOptions.PlatformCommissionRate;
            var platformCommission = Math.Round(order.Amount * commissionRate, 2);
            var sellerPayout = order.Amount - platformCommission;

            var escrow = new EscrowRecord
            {
                Id = Guid.NewGuid(),

                BookingId = null,
                MarketplaceOrderId = order.Id,
                AdjustmentRequestId = null,

                GrossAmount = order.Amount,
                CommissionRateSnapshot = commissionRate,
                PlatformCommission = platformCommission,
                ProviderPayout = sellerPayout,
                VatAmount = 0,

                PartialSettleCustomerAmt = null,
                PartialSettleProviderAmt = null,

                Status = EscrowStatus.Held,
                PaymentGatewayReference = string.IsNullOrWhiteSpace(request.PaymentGatewayReference)? $"sim_MarketplaceOrder_{order.Id}": request.PaymentGatewayReference,
                AdditionalHoldDays = _paymentOptions.DefaultEscrowHoldDays,
                EarliestReleaseAt = now.AddDays(_paymentOptions.DefaultEscrowHoldDays),

                CapturedAt = now,
                HeldAt = now,
                ReleasedAt = null,
                FrozenAt = null,
                FreezeReason = null,
                CreatedAt = now
            };

            _context.EscrowRecords.Add(escrow);

            order.Status = MarketplaceOrderStatus.PaymentCaptured;

            await _context.SaveChangesAsync(cancellationToken);

            return new MarketplaceOrderPaymentConfirmResponseDto
            {
                MarketplaceOrderId = order.Id,
                OrderStatus = order.Status.ToString(),
                OrderStatusAr = order.Status.ToArabic(),
                EscrowStatus = escrow.Status.ToString(),
                EscrowStatusAr = escrow.Status.GetStatusAr(),
                Message = "Payment captured and marketplace order escrow created"
            };
        }
    }
}