using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ProviderEarnings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ProviderEarnings.Queries.GetProviderEarningsServiceSummary;

public class GetProviderEarningsServiceSummaryQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetProviderEarningsServiceSummaryQuery, ProviderEarningsServiceSummaryDto>
{
    private static readonly HashSet<EscrowStatus> ReleasedStatuses = new() { EscrowStatus.Released };

    public async Task<ProviderEarningsServiceSummaryDto> Handle(
        GetProviderEarningsServiceSummaryQuery request, CancellationToken cancellationToken)
    {
      
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");


        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == currentUserService.UserId.Value)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Only registered providers can access earnings data.");


        var listingIds = await context.ServiceListings
            .AsNoTracking()
            .Where(l => l.ProviderProfileId == providerProfileId)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        // Convert DateOnly parameters to DateTime boundaries for database querying
        var rangeStart = request.From.ToDateTime(TimeOnly.MinValue);
        var rangeEndExclusive = request.To.ToDateTime(TimeOnly.MaxValue);

        //  Fetch all financial escrow records associated with the provider's bookings
        var escrowRows = await (
            from escrow in context.EscrowRecords.AsNoTracking()
            join booking in context.Bookings.AsNoTracking() on escrow.BookingId equals booking.Id
            where listingIds.Contains(booking.ServiceListingId)
                  && escrow.CreatedAt >= rangeStart
                  && escrow.CreatedAt <= rangeEndExclusive
            select new { escrow.GrossAmount, escrow.PlatformCommission, escrow.ProviderPayout, escrow.Status }
        ).ToListAsync(cancellationToken);

        // Business Rule Protection: If escrow data is incomplete or empty, safely return 0 amounts to UI
        if (!escrowRows.Any())
        {
            return new ProviderEarningsServiceSummaryDto(0, 0, 0, 0, 0, 0);
        }

        // 5. Calculate Basic Monetary Totals
        var grossAmount = escrowRows.Sum(r => r.GrossAmount);
        var platformCommission = escrowRows.Sum(r => r.PlatformCommission);
        var providerPayout = escrowRows.Sum(r => r.ProviderPayout);

        // 6. Split between Released and Held amounts
        var releasedAmount = escrowRows.Where(r => ReleasedStatuses.Contains(r.Status)).Sum(r => r.ProviderPayout);

        // 📑 TODO: Developer 4 Escrow Integration Hook
        // Currently, we calculate HeldAmount locally by aggregating any state that is NOT 'Released'.
        // Once Developer 4 finishes the authoritative payment gateway state machine,
        // this placeholder calculation should be replaced with a call to their dedicated escrow service.
        var heldAmount = escrowRows.Where(r => !ReleasedStatuses.Contains(r.Status)).Sum(r => r.ProviderPayout);

       
        var completedBookingsCount = await context.Bookings
            .AsNoTracking()
            .CountAsync(b => listingIds.Contains(b.ServiceListingId)
                           && b.Status == BookingStatus.Completed
                           && b.CompletionConfirmedAt != null
                           && b.CompletionConfirmedAt >= rangeStart
                           && b.CompletionConfirmedAt <= rangeEndExclusive,
                        cancellationToken);

        // 8. Return final verified data package to Angular
        return new ProviderEarningsServiceSummaryDto(
            GrossAmount: grossAmount,
            PlatformCommission: platformCommission,
            ProviderPayout: providerPayout,
            HeldAmount: heldAmount,
            ReleasedAmount: releasedAmount,
            CompletedBookingsCount: completedBookingsCount
        );
    }
}