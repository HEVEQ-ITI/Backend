using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListing;

public record DeleteServiceListingCommand(Guid Id) : IRequest;