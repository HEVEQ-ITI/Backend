using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class AdminDashboardSummaryDTO
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalProviders { get; set; }
        public int PendingServiceListings { get; set; }
        public int PendingMarketplaceListings { get; set; }
        public int PendingDocuments { get; set; }
        public int ActiveBookings { get; set; }
        public int OpenTickets { get; set; }
        public int DisputedBookings { get; set; }
        public int EscrowFrozenCount { get; set; }
    }
}
