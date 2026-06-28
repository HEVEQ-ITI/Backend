using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetDashboardSummary
{
    public class GetDashboardSummaryQueryHandler(
        UserManager<ApplicationUser> userManager,
        IApplicationDbContext context)
        : IRequestHandler<GetDashboardSummaryQuery, AdminDashboardSummaryDTO>
    {
        public async Task<AdminDashboardSummaryDTO> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            var response = new AdminDashboardSummaryDTO();

            // 1. إحصائيات المستخدمين (في استعلام واحد للأداء)
            var userStats = await userManager.Users
                .GroupBy(u => 1)
                .Select(g => new
                {
                    Total = g.Count(),
                    Active = g.Count(u => u.IsActive)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userStats != null)
            {
                response.TotalUsers = userStats.Total;
                response.ActiveUsers = userStats.Active;
            }

            // 2. إحصائيات مقدمي الخدمة
            var providers = await userManager.GetUsersInRoleAsync("Provider");
            response.TotalProviders = providers.Count;

            // 3. تجهيز مهام الجداول الأخرى (بدون استخدام await هنا لكي تنطلق جميعها في نفس الوقت)
            var pendingDocsTask = context.Documents
                .CountAsync(d => d.Status == DocumentVerificationStatus.Pending, cancellationToken);

            var pendingServicesTask = context.ServiceListings
                .CountAsync(s => s.Status == ServiceListingStatus.PendingReview, cancellationToken);

            var pendingMarketplaceTask = context.MarketplaceListings
                .CountAsync(m => m.Status == MarketplaceListingStatus.PendingReview, cancellationToken);

            var activeBookingsTask = context.Bookings
                .CountAsync(b => b.Status == BookingStatus.InProgress, cancellationToken);

            var disputedBookingsTask = context.Bookings
                .CountAsync(b => b.DisputeOpenedAt.HasValue);

            var openTicketsTask = context.Tickets
                .CountAsync(t => t.Status == TicketStatus.Open, cancellationToken);

            var escrowFrozenTask = context.EscrowRecords
                .CountAsync(e => e.Status == EscrowStatus.Frozen, cancellationToken);

            await Task.WhenAll(
                pendingDocsTask,
                pendingServicesTask,
                pendingMarketplaceTask,
                activeBookingsTask,
                disputedBookingsTask,
                openTicketsTask,
                escrowFrozenTask
            );

            response.PendingDocuments = pendingDocsTask.Result;
            response.PendingServiceListings = pendingServicesTask.Result;
            response.PendingMarketplaceListings = pendingMarketplaceTask.Result;
            response.ActiveBookings = activeBookingsTask.Result;
            response.DisputedBookings = disputedBookingsTask.Result;
            response.OpenTickets = openTicketsTask.Result;
            response.EscrowFrozenCount = escrowFrozenTask.Result;

            return response;
        }
    }
}
