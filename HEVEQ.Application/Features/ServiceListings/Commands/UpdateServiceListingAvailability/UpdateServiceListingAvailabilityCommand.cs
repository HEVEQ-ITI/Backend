using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UpdateServiceListingAvailability;

public record UpdateServiceListingAvailabilityCommand(
    Guid ListingId,
    Guid AvailabilityId,
    int DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime) : IRequest;