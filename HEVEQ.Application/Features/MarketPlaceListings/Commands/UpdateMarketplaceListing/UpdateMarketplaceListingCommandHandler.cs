using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HEVEQ.Application.Features.MarketPlace.Commands.UpdateMarketplaceListing
{
    public class UpdateMarketplaceListingCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateMarketplaceListingCommand>
    {
        public async Task Handle(UpdateMarketplaceListingCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var listing = await context.MarketplaceListings
                .FirstOrDefaultAsync(l => l.Id == command.Id, cancellationToken);
            if (listing == null)
                throw new KeyNotFoundException($"Marketplace listing with ID {command.Id} was not found.");
            if (listing.SellerId != command.SellerId)
                throw new UnauthorizedAccessException("You are not authorized to update this listing.");

            var categoryExists = await context.Categories
                .AnyAsync(c => c.Id == request.CategoryId && c.Type == CategoryType.Marketplace, cancellationToken);

            if (!categoryExists)
                throw new KeyNotFoundException($"Category with ID {request.CategoryId} was not found.");

            listing.CategoryId = request.CategoryId;
            listing.Title = request.Title;
            listing.Condition = request.Condition;
            listing.YearOfManufacture = request.YearOfManufacture;
            listing.Description = request.Description;
            listing.Specifications = request.Specifications;
            listing.Price = request.Price;
            listing.IsNegotiable = request.IsNegotiable;
            listing.TransactionMethod = request.TransactionMethod;
            listing.Governorate = request.Governorate;
            listing.District = request.District;
            listing.VideoUrl = request.VideoUrl;
            listing.Status = MarketplaceListingStatus.PendingReview;
            listing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
