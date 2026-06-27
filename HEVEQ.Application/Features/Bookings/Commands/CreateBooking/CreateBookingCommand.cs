using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CreateBooking
{
    public sealed record CreateBookingCommand(
        Guid CustomerId,
        Guid ServiceListingId,
        string JobTitle,
        string? JobDescription,
        Guid? AddressId,
        string? Governorate,
        string? District,
        string? Street,
        decimal? Latitude,
        decimal? Longitude,
        DateOnly RequestedStartDate,
        TimeOnly RequestedStartTime,
        decimal EstimatedDurationHours,
        string? SiteContactName,
        string? SiteContactPhone,
        string? AccessRequirements,
        string? SafetyNotes,
        bool AcceptOutOfZoneSurcharge
    ) : IRequest<CreateBookingResponseDto>;
}
