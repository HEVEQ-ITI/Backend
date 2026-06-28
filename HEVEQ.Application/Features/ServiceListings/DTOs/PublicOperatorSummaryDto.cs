using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record PublicOperatorSummaryDto(
        Guid Id,
        string FullName,
        string? LicenseType,
        double Rating
    );
}
