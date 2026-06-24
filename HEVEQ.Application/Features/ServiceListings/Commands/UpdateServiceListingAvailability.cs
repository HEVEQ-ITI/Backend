using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands;

public record UpdateAvailabilityRequest(
    int DayOfWeek,
    TimeOnly OpenTime,
    TimeOnly CloseTime);

public record UpdateServiceListingAvailabilityCommand(
    Guid ListingId,
    Guid AvailabilityId,
    UpdateAvailabilityRequest Request) : IRequest;

public class UpdateServiceListingAvailabilityCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<UpdateServiceListingAvailabilityCommand>
{
    public async Task Handle(UpdateServiceListingAvailabilityCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var userId = currentUserService.UserId.Value;

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        var listing = await context.ServiceListings
            .AsNoTracking()
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        if (request.Request.DayOfWeek is < 0 or > 6 || request.Request.OpenTime >= request.Request.CloseTime)
            throw new ValidationException();

        var availability = await context.ServiceListingAvailability
            .SingleOrDefaultAsync(a => a.Id == request.AvailabilityId && a.ListingId == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListingAvailability), request.AvailabilityId);

        availability.DayOfWeek = request.Request.DayOfWeek;
        availability.OpenTime = request.Request.OpenTime;
        availability.CloseTime = request.Request.CloseTime;

        await context.SaveChangesAsync(cancellationToken);
    }
}