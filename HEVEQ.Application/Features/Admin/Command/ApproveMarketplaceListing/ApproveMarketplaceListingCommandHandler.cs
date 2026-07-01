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
        
            private const int MIN_PHOTOS_REQUIRED = 1;

        public async Task<ApproveMarketplaceListingResponse> Handle(ApproveMarketplaceListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await context.MarketplaceListings
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new ApproveMarketplaceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Marketplace listing not found." };
            }

            if (listing.Status != MarketplaceListingStatus.PendingReview)
            {
                return new ApproveMarketplaceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot approve listing. Current status is {listing.Status}."
                };
            }

            if (listing.Photos == null || listing.Photos.Count < MIN_PHOTOS_REQUIRED)
            {
                return new ApproveMarketplaceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot approve listing. It must have at least {MIN_PHOTOS_REQUIRED} photo(s)."
                };
            }

            listing.Status = MarketplaceListingStatus.Active;

            await context.SaveChangesAsync(cancellationToken);

            // 6. إرسال إشعار للمزود إذا كانت خدمة الإشعارات متاحة
            //if (notificationService != null)
            //{
            //    // يمكننا سحب UserId من ProviderProfileId مباشرة كما اتفقنا سابقاً
            //    await notificationService.SendAsync(
            //        userId: listing.ProviderProfileId,
            //        title: "تم قبول عرضك في السوق",
            //        message: $"تمت الموافقة على العرض '{listing.Title}' وهو الآن نشط للبيع/التأجير."
            //    );
            //}

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
