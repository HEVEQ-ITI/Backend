using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ProviderDashboard.Queries.GetProviderDashboardSummary;

public class GetProviderDashboardSummaryQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetProviderDashboardSummaryQuery, ProviderDashboardSummaryDto>
{
    public async Task<ProviderDashboardSummaryDto> Handle(
        GetProviderDashboardSummaryQuery request, CancellationToken cancellationToken)
    {

        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var userId = currentUserService.UserId.Value;

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Only registered providers can access the dashboard summary.");


        var provider = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.Id == providerProfileId)
            .Select(p => new { p.AverageRating, p.ResponseRate, p.TrustScore, p.TrustLevel })
            .SingleAsync(cancellationToken);


        var listingCounts = await context.ServiceListings
            .AsNoTracking()
            .Where(l => l.ProviderProfileId == providerProfileId)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Approved = g.Count(l => l.Status == ServiceListingStatus.Active),
                Pending = g.Count(l => l.Status == ServiceListingStatus.PendingReview)
            })
            .SingleOrDefaultAsync(cancellationToken);

        
        var listingIds = await context.ServiceListings
            .AsNoTracking()
            .Where(l => l.ProviderProfileId == providerProfileId)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

   
        var pendingBookingRequests = await context.Bookings
            .AsNoTracking()
            .CountAsync(b => listingIds.Contains(b.ServiceListingId) && b.Status == BookingStatus.PendingProviderResponse, cancellationToken);

        var activeJobs = await context.Bookings
            .AsNoTracking()
            .CountAsync(b => listingIds.Contains(b.ServiceListingId) && (b.Status == BookingStatus.Active || b.Status == BookingStatus.InProgress), cancellationToken);

        var completedJobs = await context.Bookings
            .AsNoTracking()
            .CountAsync(b => listingIds.Contains(b.ServiceListingId) && b.Status == BookingStatus.Completed, cancellationToken);

       
        var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var earningsThisMonth = await (from escrow in context.EscrowRecords
                                       join booking in context.Bookings on escrow.BookingId equals booking.Id
                                       where listingIds.Contains(booking.ServiceListingId)
                                             && escrow.ReleasedAt != null
                                             && escrow.ReleasedAt >= monthStart
                                       select escrow.ProviderPayout)
                                       .SumAsync(payout => (decimal?)payout, cancellationToken) ?? 0m;

    
        return new ProviderDashboardSummaryDto(
            TotalServiceListings: listingCounts?.Total ?? 0,
            ApprovedServiceListings: listingCounts?.Approved ?? 0,
            PendingServiceListings: listingCounts?.Pending ?? 0,
            PendingBookingRequests: pendingBookingRequests,
            ActiveJobs: activeJobs,
            CompletedJobs: completedJobs,
            AverageRating: provider.AverageRating,
            ResponseRate: provider.ResponseRate,
            TrustScore: provider.TrustScore,
            TrustLevel: ((TrustLevel)provider.TrustLevel).ToString(),
            EarningsThisMonth: earningsThisMonth
        );
    }
}