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

public record DeleteServiceListingPhotoCommand(Guid ListingId, Guid PhotoId) : IRequest;

public class DeleteServiceListingPhotoCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteServiceListingPhotoCommand>
{
    public async Task Handle(DeleteServiceListingPhotoCommand request, CancellationToken cancellationToken)
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

        var photo = await context.ServiceListingPhotos
            .SingleOrDefaultAsync(p => p.Id == request.PhotoId && p.ListingId == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListingPhoto), request.PhotoId);

        context.ServiceListingPhotos.Remove(photo);
        await context.SaveChangesAsync(cancellationToken);
    }
}