using MediatR;
using System;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UpdateServiceListing;

public record UpdateServiceListingCommand(
    Guid Id,
    int CategoryId,
    string Title,
    string Description,
    string? Tags,
    string? EquipmentModel,
    string? EquipmentCapacity,
    EquipmentCondition? EquipmentCondition,
    int? YearOfManufacture,
    string? EquipmentRegistrationNumber,
    decimal HourlyRate,
    decimal? DailyRate,
    int MinimumBookingHours
) : IRequest;