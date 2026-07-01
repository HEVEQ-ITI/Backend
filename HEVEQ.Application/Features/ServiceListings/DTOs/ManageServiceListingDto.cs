using HEVEQ.Application.Features.Documents.DTOs;
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
        int? AiRiskScore,                 
        string? AiRiskLevel,              
        string? AiRecommendation,
        List<ServiceListingPhotoDto> Photos,
        List<ServiceListingOperatorDto> Operators,
        List<ServiceListingAvailabilityDto> Availability,
        List<BlackoutDateDto> BlackoutDates,
        List<DocumentDto> Documents,
        bool CanSubmitForReview,
        List<string> MissingRequirements);
}
