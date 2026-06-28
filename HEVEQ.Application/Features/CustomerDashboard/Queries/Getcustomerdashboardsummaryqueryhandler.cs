using MediatR;
using Microsoft.EntityFrameworkCore;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.CustomerDashboard.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.CustomerDashboard.Queries.GetCustomerDashboardSummary;

public class GetCustomerDashboardSummaryQueryHandler
    : IRequestHandler<GetCustomerDashboardSummaryQuery, CustomerDashboardSummaryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetCustomerDashboardSummaryQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<CustomerDashboardSummaryDto> Handle(
        GetCustomerDashboardSummaryQuery request,
        CancellationToken cancellationToken)
    {
        // ── Step 1: get current user ──────────────────────────────────────
        // UserId comes from JWT token via CurrentUserService — never hardcoded
        var userId = _currentUser.UserId
            ?? throw new ForbiddenAccessException("User is not authenticated.");

        // ── Step 2: get CustomerProfile for trust data ────────────────────
        // TrustScore and RequiresAdditionalVerification live on CustomerProfile
        var profile = await _context.CustomerProfiles
            .Where(p => p.UserId == userId)
            .Select(p => new { p.TrustScore, p.RequiresAdditionalVerification })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(CustomerProfile), userId);

        // ── Step 3: booking counters ──────────────────────────────────────
        // Booking.CustomerId is FK to ApplicationUser.Id (confirmed from BookingConfiguration)
        // All counts are separate DB queries — each is a lightweight COUNT(*)
        var bookingsBase = _context.Bookings
            .Where(b => b.CustomerId == userId);

        // Active: customer has a confirmed/running booking
        var activeBookings = await bookingsBase
            .CountAsync(b =>
                b.Status == BookingStatus.Active ||
                b.Status == BookingStatus.InProgress ||
                b.Status == BookingStatus.ConfirmedPendingPayment,
                cancellationToken);

        // Pending: waiting for provider response OR waiting for customer to confirm completion
        var pendingBookings = await bookingsBase
            .CountAsync(b =>
                b.Status == BookingStatus.PendingProviderResponse ||
                b.Status == BookingStatus.PendingCustomerConfirmation,
                cancellationToken);

        // Completed: fully done (all resolution variants)
        var completedBookings = await bookingsBase
            .CountAsync(b =>
                b.Status == BookingStatus.Completed ||
                b.Status == BookingStatus.ResolvedReleased ||
                b.Status == BookingStatus.ResolvedRefunded,
                cancellationToken);

        // Open disputes: booking is currently in dispute
        var openDisputes = await bookingsBase
            .CountAsync(b => b.Status == BookingStatus.Disputed,
                cancellationToken);

        // ── Step 4: marketplace purchases ────────────────────────────────
        // MarketplaceOrder.BuyerId is FK to ApplicationUser.Id
        // Count all orders the customer has ever placed (no status filter)
        var marketplacePurchases = await _context.MarketplaceOrders
            .CountAsync(o => o.BuyerId == userId, cancellationToken);

        // ── Step 5: unread notifications ─────────────────────────────────
        // Notification.UserId is FK to ApplicationUser.Id
        var unreadNotifications = await _context.Notifications
            .CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);

        // ── Step 6: assemble and return ───────────────────────────────────
        return new CustomerDashboardSummaryDto
        {
            ActiveBookings = activeBookings,
            PendingBookings = pendingBookings,
            CompletedBookings = completedBookings,
            OpenDisputes = openDisputes,
            MarketplacePurchases = marketplacePurchases,
            UnreadNotifications = unreadNotifications,
            TrustScore = profile.TrustScore,
            RequiresAdditionalVerification = profile.RequiresAdditionalVerification
        };
    }
}