using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HEVEQ.Application.Features.ServiceListings.DTOs;

public record PublicServiceListingDetailDto(
    Guid Id,
    string Title,
    string Description,
    string CategoryName,
    string? EquipmentModel,
    string? EquipmentCapacity,
    EquipmentCondition? EquipmentCondition,
    int? YearOfManufacture,
    decimal HourlyRate,
    decimal DailyRate,
    int MinimumBookingHours,
    List<string> Photos,
    List<PublicAvailabilityDto> Availability,
    PublicProviderSummaryDto Provider,
    List<PublicOperatorSummaryDto> Operators,
    bool CanRequestBooking
);

