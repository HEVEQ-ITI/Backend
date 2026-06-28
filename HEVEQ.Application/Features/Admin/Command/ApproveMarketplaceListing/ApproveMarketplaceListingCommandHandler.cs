using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ApproveMarketplaceListing
{
    public class ApproveMarketplaceListingCommandHandler(IApplicationDbContext context)
        : IRequestHandler<ApproveMarketplaceListingCommand, ApproveMarketplaceListingResponse>
    {
        public async Task<ApproveMarketplaceListingResponse> Handle(ApproveMarketplaceListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await context.MarketplaceListings
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new ApproveMarketplaceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Marketplace listing not found." };
            }

            // 2. حماية الحالة (Business Rule)
            if (listing.Status != MarketplaceListingStatus.PendingReview)
            {
                return new ApproveMarketplaceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot approve listing. Current status is {listing.Status}."
                };
            }

            listing.Status = MarketplaceListingStatus.Active;

            listing.EmbeddingStatus = EmbeddingStatus.Pending;

            await context.SaveChangesAsync(cancellationToken);

            return new ApproveMarketplaceListingResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = listing.Id,
                Status = "Active", 
                StatusAr = "نشط",
                Message = "Marketplace listing approved successfully"
            };
        }
    }
}
