using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;


namespace HEVEQ.Application.Features.MarketPlace.Commands.CreateMarketPlaceListing
{
    public class CreateMarketPlaceListingCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateMarketPlaceListingCommand,Guid>
    {
        public async Task<Guid> Handle(CreateMarketPlaceListingCommand command, CancellationToken cancellationToken)
        {
            //if (!_currentUserService.UserId.HasValue)
            //    throw new ForbiddenAccessException("User is not authenticated.");

            //var userId = _currentUserService.UserId.Value;

            var request = command.Request;

            var categoryExists = await context.Categories
             .AnyAsync(c => c.Id == request.CategoryId && c.Type == CategoryType.Marketplace, cancellationToken);

            if (!categoryExists) throw new Exception($"can't find category by id {request.CategoryId}");
            //throw new NotFoundException(nameof(Category), request.CategoryId);

            var listing = new MarketplaceListing
            {
                SellerId = command.SellerId,
                CategoryId = request.CategoryId,
                Title = request.Title,
                Condition = request.Condition,
                YearOfManufacture = request.YearOfManufacture,
                Description = request.Description,
                Specifications = request.Specifications,
                Price = request.Price,
                IsNegotiable = request.IsNegotiable,
                TransactionMethod = request.TransactionMethod,
                Governorate = request.Governorate,
                District = request.District,
                VideoUrl = request.VideoUrl,        
                Status = MarketplaceListingStatus.PendingReview,
                EmbeddingStatus = EmbeddingStatus.Pending,
                SubmissionCount = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.MarketplaceListings.Add(listing);
            await context.SaveChangesAsync(cancellationToken);
           
            return listing.Id;



        }
    }
}
