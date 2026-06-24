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

namespace HEVEQ.Application.Features.ServiceListings.Commands;

public record AddPhotoRequest(
    string PhotoUrl,
    int DisplayOrder);

public record AddServiceListingPhotoCommand(
    Guid ListingId,
    AddPhotoRequest Request) : IRequest<Guid>;

public class AddServiceListingPhotoCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddServiceListingPhotoCommand, Guid>
{
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

        var listing = await context.ServiceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new NotFoundException(nameof(ServiceListing), request.ListingId);

        if (listing.ProviderProfileId != providerProfileId)
            throw new ForbiddenAccessException("This listing does not belong to the current provider.");

        if (string.IsNullOrWhiteSpace(request.Request.PhotoUrl))
            throw new ValidationException();

        if (listing.Status == ServiceListingStatus.Active)
        {
            listing.Status = ServiceListingStatus.PendingReview;
        }

        var currentPhotosCount = await context.ServiceListingPhotos
            .CountAsync(p => p.ListingId == request.ListingId, cancellationToken);

        listing.QsPhotos = (currentPhotosCount + 1) >= 3 ? 100 : 50;
        listing.UpdatedAt = DateTime.UtcNow;

        var photo = new ServiceListingPhoto
        {
            Id = Guid.NewGuid(),
            ListingId = request.ListingId,
            PhotoUrl = request.Request.PhotoUrl,
            DisplayOrder = request.Request.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        context.ServiceListingPhotos.Add(photo);
        await context.SaveChangesAsync(cancellationToken);

        return photo.Id;
    }
}