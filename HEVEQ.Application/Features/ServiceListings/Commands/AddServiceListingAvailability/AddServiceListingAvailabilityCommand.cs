using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddServiceListingAvailability
{
    public record AddServiceListingAvailabilityCommand(
        Guid ListingId,
        int DayOfWeek,
        TimeOnly OpenTime,
        TimeOnly CloseTime) : IRequest<ServiceListingAvailabilityDto>;
}
