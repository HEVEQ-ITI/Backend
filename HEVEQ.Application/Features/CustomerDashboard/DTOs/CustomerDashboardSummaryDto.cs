using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.CustomerDashboard.DTOs;

public class CustomerDashboardSummaryDto
{
    // Booking counters
    public int ActiveBookings { get; set; }
    public int PendingBookings { get; set; }
    public int CompletedBookings { get; set; }
    public int OpenDisputes { get; set; }

    // Marketplace
    public int MarketplacePurchases { get; set; }

    // Notifications
    public int UnreadNotifications { get; set; }

    // From CustomerProfile — read-only trust data
    public decimal TrustScore { get; set; }
    public bool RequiresAdditionalVerification { get; set; }
}
