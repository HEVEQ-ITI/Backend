using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteBlackoutDate;

public record DeleteBlackoutDateCommand(Guid ListingId, Guid BlackoutDateId) : IRequest;