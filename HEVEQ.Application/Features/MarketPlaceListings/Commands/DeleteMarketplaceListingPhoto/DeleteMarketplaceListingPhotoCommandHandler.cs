using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketplaceListingPhoto
{
    public class DeleteMarketplaceListingPhotoCommandHandler(IApplicationDbContext context,ICurrentUserService currentUser) : IRequestHandler<DeleteMarketplaceListingPhotoCommand>
    {
        private const int MinPhotosWhileUnderReviewOrActive = 3;
        public async Task Handle(DeleteMarketplaceListingPhotoCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");


            var listing = await context.MarketplaceListings
                .Include(l => l.Photos)
                .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceListing), request.ListingId);


            if (listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to delete this photo.");


            var photo = listing.Photos.FirstOrDefault(p => p.Id == request.PhotoId)
               ?? throw new NotFoundException(nameof(MarketplaceListingPhoto), request.PhotoId);

            if (listing.Status != MarketplaceListingStatus.Draft &&
                  listing.Photos.Count <= MinPhotosWhileUnderReviewOrActive)
            {
                throw new ValidationException("Photos",
                    $"A listing under review or active must keep at least {MinPhotosWhileUnderReviewOrActive} photos.");
            }

            context.MarketplaceListingPhotos.Remove(photo);
            listing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
        }

    }
}
