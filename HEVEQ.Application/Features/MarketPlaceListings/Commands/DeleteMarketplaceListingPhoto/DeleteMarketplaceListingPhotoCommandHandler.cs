using HEVEQ.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketplaceListingPhoto
{
    public class DeleteMarketplaceListingPhotoCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteMarketplaceListingPhotoCommand>
    {
        public async Task Handle(DeleteMarketplaceListingPhotoCommand request, CancellationToken cancellationToken)
        {
            var currentUserId= Guid.Parse("FC6FF724-CED5-468A-6964-08DED0682657");

            var listing = await context.MarketplaceListings
            .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken);

            if (listing is null)
                throw new KeyNotFoundException("Not Found Listing");


            var photo = await context.MarketplaceListingPhotos
            .FirstOrDefaultAsync(p => p.Id == request.PhotoId && p.ListingId == request.ListingId, cancellationToken); 

            if (photo == null)
                throw new KeyNotFoundException("Photo not found or doesn't belong to this listing.");

            if (photo.Listing.SellerId != currentUserId)
                throw new UnauthorizedAccessException("You are not authorized to delete this photo.");

            photo.Listing.UpdatedAt = DateTime.UtcNow;

            context.MarketplaceListingPhotos.Remove(photo);
            await context.SaveChangesAsync(cancellationToken);
        }

    }
}
