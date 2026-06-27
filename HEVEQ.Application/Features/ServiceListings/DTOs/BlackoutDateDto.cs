using System;

namespace HEVEQ.Application.Features.ServiceListings.DTOs;

public record BlackoutDateDto(Guid Id, Guid ListingId, Guid? OperatorId, DateOnly Date, string? Reason);