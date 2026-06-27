using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace HEVEQ.Application.Features.MarketPlace.Commands.CreateMarketPlaceListing
{
    public class CreateMarketPlaceListingCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<CreateMarketPlaceListingCommand, CreateMarketplaceListingResponse>
    {
        //public async Task<Guid> Handle(CreateMarketPlaceListingCommand command, CancellationToken cancellationToken)
        //{
        //    //if (!_currentUserService.UserId.HasValue)
        //    //    throw new ForbiddenAccessException("User is not authenticated.");

        //    //var userId = _currentUserService.UserId.Value;

        //    var request = command.Request;

        //    var categoryExists = await context.Categories
        //     .AnyAsync(c => c.Id == request.CategoryId && c.Type == CategoryType.Marketplace, cancellationToken);

        //    if (!categoryExists) throw new Exception($"can't find category by id {request.CategoryId}");
        //    //throw new NotFoundException(nameof(Category), request.CategoryId);

        //    var listing = new MarketplaceListing
        //    {
        //        SellerId = command.SellerId,
        //        CategoryId = request.CategoryId,
        //        Title = request.Title,
        //        Condition = request.Condition,
        //        YearOfManufacture = request.YearOfManufacture,
        //        Description = request.Description,
        //        Specifications = request.Specifications,
        //        Price = request.Price,
        //        IsNegotiable = request.IsNegotiable,
        //        TransactionMethod = request.TransactionMethod,
        //        Governorate = request.Governorate,
        //        District = request.District,
        //        VideoUrl = request.VideoUrl,        
        //        Status = MarketplaceListingStatus.PendingReview,
        //        EmbeddingStatus = EmbeddingStatus.Pending,
        //        SubmissionCount = 1,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow
        //    };
        //    context.MarketplaceListings.Add(listing);
        //    await context.SaveChangesAsync(cancellationToken);

        //    return listing.Id;



        //}
        private static readonly DocumentType[] SellerVerificationDocumentTypes =
       {
            DocumentType.CommercialRegistration,
            DocumentType.TaxCard
        };
        public async Task<CreateMarketplaceListingResponse> Handle(CreateMarketPlaceListingCommand command, CancellationToken cancellationToken)
        {

            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var sellerId = currentUser.UserId.Value;

            switch (currentUser.Role)
            {
                case "Provider":
                    var hasProviderProfile = await context.ProviderProfiles
                        .AnyAsync(p => p.UserId == sellerId, cancellationToken);
                    if (!hasProviderProfile)
                        throw new ForbiddenAccessException("Only a registered provider can create a marketplace listing.");
                    break;

                case "Customer":
                    var isVerifiedSeller = await context.Documents.AnyAsync(d =>
                        d.UserId == sellerId &&
                        d.Status == DocumentVerificationStatus.Approved &&
                        SellerVerificationDocumentTypes.Contains(d.DocumentType),
                        cancellationToken);
                    if (!isVerifiedSeller)
                        throw new ForbiddenAccessException(
                            "Only verified sellers can create marketplace listings. Upload a business verification document first.");
                    break;

                default:
                    throw new ForbiddenAccessException("Only providers or verified sellers can create marketplace listings.");
            }

            var request = command.Request;

            var categoryExists = await context.Categories
                .AnyAsync(c => c.Id == request.CategoryId && c.Type == CategoryType.Marketplace, cancellationToken);

            if (!categoryExists)
                throw new NotFoundException(nameof(Category), request.CategoryId);

            var listing = new MarketplaceListing
            {
                SellerId = sellerId,
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
                Status = MarketplaceListingStatus.Draft,
                EmbeddingStatus = EmbeddingStatus.Pending,
                SubmissionCount = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.MarketplaceListings.Add(listing);
            await context.SaveChangesAsync(cancellationToken);

            return new CreateMarketplaceListingResponse(
                listing.Id,
                listing.Status.ToString(),
                listing.Status.ToArabic(),
                "AddPhotos");
        }
    }
}
