using System;
using System.Collections.Generic;

namespace HEVEQ.Application.Features.ServiceListings.DTOs;

public record PublicServiceListingDto(
    Guid Id,
    string Title,
    string CategoryName,
    string ProviderCompany,
    string? CoverPhotoUrl,
    decimal HourlyRate,
    decimal DailyRate,
    int MinimumBookingHours,
    string Location,
    int ServiceRadiusKm,
    double AverageRating,
    int TotalReviewsCount,
    string TrustLevel,
    string AvailabilityText,
    string Status,
    string StatusAr
);

public record PublicPaginatedList<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount
);