using HEVEQ.Application.Features.ServiceListings.DTOs;
using MediatR;
using System;

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddBlackoutDate;

public record AddBlackoutDateCommand(
    Guid ListingId,
    DateOnly Date,
    string? Reason,
    Guid? OperatorId) : IRequest<BlackoutDateDto>;