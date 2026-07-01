using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceEarningsSummary
{
    public class GetMarketplaceEarningsSummaryQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<GetMarketplaceEarningsSummaryQuery, MarketplaceEarningsSummaryDto>
    {
        public async Task<MarketplaceEarningsSummaryDto> Handle(GetMarketplaceEarningsSummaryQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var sellerId = currentUser.UserId.Value;

            // EscrowRecord has no SellerId — resolve via MarketplaceOrder.Listing.SellerId.
            var escrowQuery = context.EscrowRecords
                .Where(e => e.MarketplaceOrderId.HasValue
                         && e.MarketplaceOrder!.Listing.SellerId == sellerId);

            // Date range filters against when the escrow row was created
            // (i.e. when the order was placed/paid), not the release date.
            if (request.From.HasValue)
            {
                var from = request.From.Value.ToDateTime(TimeOnly.MinValue);
                escrowQuery = escrowQuery.Where(e => e.CreatedAt >= from);
            }

            if (request.To.HasValue)
            {
                var to = request.To.Value.ToDateTime(TimeOnly.MaxValue);
                escrowQuery = escrowQuery.Where(e => e.CreatedAt <= to);
            }

            var records = await escrowQuery
                .Select(e => new
                {
                    e.GrossAmount,
                    e.PlatformCommission,
                    e.ProviderPayout,
                    e.Status
                }).ToListAsync(cancellationToken);

            // CompletedOrdersCount from the order side (not escrow) — escrow status
            // Released doesn't always mean the order itself reached Completed.
            var completedCount = await context.MarketplaceOrders
                .Where(o => o.Listing.SellerId == sellerId &&
                            o.Status == MarketplaceOrderStatus.Completed)
                .CountAsync(cancellationToken);

            // Apply the same date range to completed orders.
            var completedQuery = context.MarketplaceOrders
                .Where(o => o.Listing.SellerId == sellerId
                         && o.Status == MarketplaceOrderStatus.Completed);

            if (request.From.HasValue)
            {
                var from = request.From.Value.ToDateTime(TimeOnly.MinValue);
                completedQuery = completedQuery.Where(o => o.ConfirmedByBuyerAt >= from);
            }

            if (request.To.HasValue)
            {
                var to = request.To.Value.ToDateTime(TimeOnly.MaxValue);
                completedQuery = completedQuery.Where(o => o.ConfirmedByBuyerAt <= to);
            }

            completedCount = await completedQuery.CountAsync(cancellationToken);

            return new MarketplaceEarningsSummaryDto
            {
                GrossSales = records.Sum(r => r.GrossAmount),
                PlatformCommission = records.Sum(r => r.PlatformCommission),
                SellerPayout = records.Sum(r => r.ProviderPayout),
                HeldAmount = records
                    .Where(r => r.Status == EscrowStatus.Held)
                    .Sum(r => r.ProviderPayout),
                ReleasedAmount = records
                    .Where(r => r.Status == EscrowStatus.Released)
                    .Sum(r => r.ProviderPayout),
                CompletedOrdersCount = completedCount
            };
        }
    }
}
