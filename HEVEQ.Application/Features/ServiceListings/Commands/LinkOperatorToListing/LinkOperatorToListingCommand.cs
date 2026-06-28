using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.LinkOperatorToListing;

public record LinkOperatorToListingCommand(Guid ListingId, Guid OperatorId) : IRequest<LinkOperatorResultDto>;