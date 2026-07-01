using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public class MarketplaceEarningsSummaryDto
    {
        public decimal GrossSales { get; set; }
        public decimal PlatformCommission { get; set; }
        public decimal SellerPayout { get; set; }
        public decimal HeldAmount { get; set; }
        public decimal ReleasedAmount { get; set; }
        public int CompletedOrdersCount { get; set; }
    }
}
