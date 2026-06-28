using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListingAvailability;

public record DeleteServiceListingAvailabilityCommand(Guid ListingId, Guid AvailabilityId) : IRequest;