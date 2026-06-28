using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Commands.CreateServiceListing
{
    public record CreateServiceListingCommand(
        int CategoryId, string Title, string Description, string? Tags,
        string? EquipmentModel, string? EquipmentCapacity, EquipmentCondition? EquipmentCondition,
        int? YearOfManufacture, string? EquipmentRegistrationNumber,
        decimal HourlyRate, decimal? DailyRate, int MinimumBookingHours)
        : IRequest<CreateServiceListingResultDto>;
}
