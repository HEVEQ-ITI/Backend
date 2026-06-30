using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record PublicReviewsSummaryDto(
        double AverageRating,
        int TotalReviewsCount
    );
}
