using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketPlaceListing
{
    public class DeleteMarketPlaceListingCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<DeleteMarketPlaceListingCommand>
    {
        private const int MinPhotosWhileUnderReviewOrActive = 3;

        public async Task Handle(DeleteMarketPlaceListingCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var listing = await context.MarketplaceListings
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(MarketplaceListing), request.Id);

            if (listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to delete this listing.");

            listing.Status = MarketplaceListingStatus.Inactive;
            listing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
