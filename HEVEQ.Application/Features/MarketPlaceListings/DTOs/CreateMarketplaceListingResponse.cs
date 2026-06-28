using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceListings.DTOs
{
    public record CreateMarketplaceListingResponse(
        Guid Id,
        string Status,
        string StatusAr,
        string NextStep);
    
}
