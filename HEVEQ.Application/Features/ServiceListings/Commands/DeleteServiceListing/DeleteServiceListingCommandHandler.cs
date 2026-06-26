using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListing;

public class DeleteServiceListingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteServiceListingCommand>
{
    private static readonly int[] BlockingBookingStatuses = [0, 1, 2, 3, 4, 5, 7, 14, 15];

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

        var hasActiveBooking = await context.Bookings
            .AsNoTracking()
            .AnyAsync(b => b.ServiceListingId == request.Id && BlockingBookingStatuses.Contains((int)b.Status), cancellationToken);

        if (hasActiveBooking)
            throw new ValidationException("Booking", "This listing cannot be removed while it has an active booking.");

        listing.Status = ServiceListingStatus.Inactive;
        listing.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}