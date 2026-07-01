using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public record MarketplaceEscrowDto
    {
        public Guid OrderId { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal PlatformCommission { get; set; }

        public decimal ProviderPayout { get; set; }

        public string Status { get; set; } = string.Empty;

        public string StatusAr { get; set; } = string.Empty;

        public DateTime? CapturedAt { get; set; }

        public DateTime? ReleasedAt { get; set; }

        public DateTime? FrozenAt { get; set; }
    }
}
