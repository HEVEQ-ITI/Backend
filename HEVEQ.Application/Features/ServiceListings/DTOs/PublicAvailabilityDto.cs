using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record PublicAvailabilityDto(
        DateTime Date,
        bool IsAvailable,
        decimal? PriceOverride
    );
}
