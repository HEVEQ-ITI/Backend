using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceEscrow
{
    public class GetMarketplaceEscrowQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<GetMarketplaceEscrowQuery, MarketplaceEscrowDto>
    {
        public async Task<MarketplaceEscrowDto> Handle(GetMarketplaceEscrowQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                 .AsNoTracking()
                 .Include(o => o.Listing)
                 .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                 ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            var userId = currentUser.UserId.Value;
            var isBuyer = order.BuyerId == userId;
            var isSeller = order.Listing.SellerId == userId;
            var isAdmin = currentUser.Role == "Admin";


            if (!isBuyer && !isSeller && !isAdmin)
                throw new ForbiddenAccessException("You are not allowed to view this escrow.");

            var escrow = await context.EscrowRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.MarketplaceOrderId == order.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(EscrowRecord), request.OrderId);

            return new MarketplaceEscrowDto
            {
                OrderId = order.Id,
                GrossAmount = escrow.GrossAmount,
                PlatformCommission = escrow.PlatformCommission,
                ProviderPayout = escrow.ProviderPayout,
                Status = escrow.Status.ToString(),
                StatusAr = escrow.Status.ToArabic(),
                CapturedAt = escrow.CapturedAt,
                ReleasedAt = escrow.ReleasedAt,
                FrozenAt = escrow.FrozenAt
            };
        }
    }
}
