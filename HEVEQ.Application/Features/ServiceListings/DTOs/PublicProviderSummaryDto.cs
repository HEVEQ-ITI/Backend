using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record PublicProviderSummaryDto(
        string CompanyName,
        double AverageRating,
        int CompletedBookingsCount,
        int TrustScore,
        string TrustLevel
    );
}
