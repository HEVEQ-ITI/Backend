using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddServiceListingPhoto;

public class AddServiceListingPhotoCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddServiceListingPhotoCommand, Guid>
{
    private const int MaxPhotos = 10;

    public async Task<Guid> Handle(AddServiceListingPhotoCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenAccessException("Only registered providers can manage photos.");

        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        var currentPhotosCount = await context.ServiceListingPhotos
            .AsNoTracking()
            .CountAsync(p => p.ListingId == request.ListingId, cancellationToken);

        if (currentPhotosCount >= MaxPhotos)
            throw new ValidationException($"A listing cannot have more than {MaxPhotos} photos.");

        var photo = new ServiceListingPhoto
        {
            Id = Guid.NewGuid(),
            ListingId = request.ListingId,
            PhotoUrl = request.PhotoUrl,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        listing.UpdatedAt = DateTime.UtcNow;

        context.ServiceListingPhotos.Add(photo);
        await context.SaveChangesAsync(cancellationToken);

        return photo.Id;
    }
}