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

public record DeleteServiceListingCommand(Guid Id) : IRequest;

public class DeleteServiceListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteServiceListingCommand>
{
    public async Task Handle(DeleteServiceListingCommand request, CancellationToken cancellationToken)
    {
        if (!currentUserService.UserId.HasValue)
            throw new ForbiddenAccessException("User is not authenticated.");

        var userId = currentUserService.UserId.Value;

        var providerProfileId = await context.ProviderProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (providerProfileId == Guid.Empty)
            throw new ForbiddenAccessException("Only registered providers can manage service listings.");

        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.Id);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        var hasActiveBookings = await context.Bookings
            .AnyAsync(b => b.ServiceListingId == request.Id, cancellationToken);

        if (hasActiveBookings)
            throw new ValidationException("Booking", "Cannot delete this listing because it has past or upcoming bookings associated with it.");

        var photos = await context.ServiceListingPhotos
            .Where(p => p.ListingId == request.Id)
            .ToListAsync(cancellationToken);
        if (photos.Any()) context.ServiceListingPhotos.RemoveRange(photos);

        var availabilities = await context.ServiceListingAvailability
            .Where(a => a.ListingId == request.Id)
            .ToListAsync(cancellationToken);
        if (availabilities.Any()) context.ServiceListingAvailability.RemoveRange(availabilities);

        var linkedOperators = await context.ServiceListingOperators
            .Where(o => o.ListingId == request.Id)
            .ToListAsync(cancellationToken);
        if (linkedOperators.Any()) context.ServiceListingOperators.RemoveRange(linkedOperators);

        var blackoutDates = await context.BlackoutDates
            .Where(b => b.ListingId == request.Id)
            .ToListAsync(cancellationToken);
        if (blackoutDates.Any()) context.BlackoutDates.RemoveRange(blackoutDates);

        context.ServiceListings.Remove(listing);

        await context.SaveChangesAsync(cancellationToken);
    }
}