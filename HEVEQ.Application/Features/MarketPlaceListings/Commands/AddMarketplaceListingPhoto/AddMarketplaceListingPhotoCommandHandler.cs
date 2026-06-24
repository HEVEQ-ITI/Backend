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
    public class AddMarketplaceListingPhotoCommandHandler(IApplicationDbContext context) : IRequestHandler<AddMarketplaceListingPhotoCommand, Guid>
    {
        public async Task<Guid> Handle(AddMarketplaceListingPhotoCommand request, CancellationToken cancellationToken)
        {
            var currentUserId= Guid.Parse("FC6FF724-CED5-468A-6964-08DED0682657");
            var listing = await context.MarketplaceListings
            .SingleOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
            ?? throw new KeyNotFoundException($"Marketplace listing with ID {request.ListingId} was not found.");

            if (listing.SellerId != currentUserId)
                throw new UnauthorizedAccessException("This listing does not belong to you.");

            if (listing.Status == MarketplaceListingStatus.Active)
            {
                listing.Status = MarketplaceListingStatus.PendingReview;
            }
            listing.UpdatedAt = DateTime.UtcNow;

            var photo = new MarketplaceListingPhoto
            {
                Id = Guid.NewGuid(),
                ListingId = request.ListingId,
                PhotoUrl = request.Request.PhotoUrl, 
                DisplayOrder = request.Request.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };

            context.MarketplaceListingPhotos.Add(photo);
            await context.SaveChangesAsync(cancellationToken);

            return photo.Id;
        }
    }
}
