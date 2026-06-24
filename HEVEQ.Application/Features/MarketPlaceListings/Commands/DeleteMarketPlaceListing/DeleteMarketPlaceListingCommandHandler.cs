using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketPlaceListing
{
    public class DeleteMarketPlaceListingCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteMarketPlaceListingCommand>
    {

        public async Task Handle(DeleteMarketPlaceListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await context.MarketplaceListings
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Marketplace listing with ID {request.Id} was not found.");
            if (listing.SellerId != request.SellerId)
                throw new UnauthorizedAccessException("You are not authorized to delete this listing.");

            listing.Status = MarketplaceListingStatus.Inactive;
            listing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
