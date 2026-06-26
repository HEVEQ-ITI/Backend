using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UnlinkOperatorFromListing;

public record UnlinkOperatorFromListingCommand(Guid ListingId, Guid OperatorId) : IRequest;