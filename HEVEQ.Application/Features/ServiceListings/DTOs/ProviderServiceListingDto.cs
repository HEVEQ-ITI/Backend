using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record ProviderServiceListingDto(
        Guid Id,
        string Title,
        string CategoryName,
        string? CoverPhotoUrl,
        decimal HourlyRate,
        string Status,
        string StatusAr,
        int QualityScore,
        int? AiRiskScore,        
        string? AiRiskLevel,
        int PhotosCount,
        int OperatorsCount,
        int AvailabilityCount,
        DateTime CreatedAt
    );
}
