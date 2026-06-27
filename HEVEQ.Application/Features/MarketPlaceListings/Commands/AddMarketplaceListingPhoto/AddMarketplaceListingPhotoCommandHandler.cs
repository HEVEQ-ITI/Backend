using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.AddMarketplaceListingPhoto
{
    public class AddMarketplaceListingPhotoCommandHandler(IApplicationDbContext context,ICurrentUserService currentUser) : IRequestHandler<AddMarketplaceListingPhotoCommand, Guid>
    {
        private const int MinPhotosForReview = 3;
        private const int MaxPhotos = 12;
        public async Task<Guid> Handle(AddMarketplaceListingPhotoCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var listing = await context.MarketplaceListings
                .Include(l => l.Photos)
                .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceListing), request.ListingId);

            if (listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("This listing does not belong to you.");

            if (listing.Photos.Count >= MaxPhotos)
                throw new ValidationException("Photos", $"A listing cannot have more than {MaxPhotos} photos.");

           

            var photo = new MarketplaceListingPhoto
            {
                Id = Guid.NewGuid(),
                ListingId = request.ListingId,
                PhotoUrl = request.Request.PhotoUrl, 
                DisplayOrder = request.Request.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };

            context.MarketplaceListingPhotos.Add(photo);

            if (listing.Status == MarketplaceListingStatus.Draft &&
                listing.Photos.Count + 1 >= MinPhotosForReview)
            {
                listing.Status = MarketplaceListingStatus.PendingReview;
            }

            // A previously Active listing gets a new photo -> back through review.
            else if (listing.Status == MarketplaceListingStatus.Active)
            {
                listing.Status = MarketplaceListingStatus.PendingReview;
            }

            listing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return photo.Id;
        }
    }
}
