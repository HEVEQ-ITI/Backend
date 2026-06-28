using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record PublicSearchResultDto(
        string Query,
        string Context,
        List<PublicServiceListingDto> Items,
        int ResultCount,
        bool HasLowConfidence,
        string? Message);
}
