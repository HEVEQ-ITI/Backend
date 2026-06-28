using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.DTOs
{
    public class PaginatedPendingMarketplaceResponse
    {
        public List<PendingMarketplaceListingDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
