using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.DTOs
{
    public record ManageServiceListingDto(
        Guid Id,
        string Title,
        string Description,
        int CategoryId,
        string Status,
        string StatusAr,
        string? AdminRejectionNote,
        int? QualityScore,
        List<ServiceListingPhotoDto> Photos,
        List<ServiceListingOperatorDto> Operators,
        List<ServiceListingAvailabilityDto> Availability,
        List<BlackoutDateDto> BlackoutDates,
        List<string> Documents,
        bool CanSubmitForReview,
        List<string> MissingRequirements);
}
