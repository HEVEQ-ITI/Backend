using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RejectMarketplaceListing
{
    public class RejectMarketplaceListingCommandHandler(IApplicationDbContext context)
        : IRequestHandler<RejectMarketplaceListingCommand, RejectMarketplaceListingResponse>
    {
        public async Task<RejectMarketplaceListingResponse> Handle(RejectMarketplaceListingCommand request, CancellationToken cancellationToken)
        {

            var listing = await context.MarketplaceListings
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new RejectMarketplaceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Marketplace listing not found." };
            }

            if (listing.Status != MarketplaceListingStatus.PendingReview)
            {
                return new RejectMarketplaceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot reject listing. Current status is {listing.Status}."
                };
            }

            listing.Status = MarketplaceListingStatus.Rejected;
            listing.AdminRejectionNote = request.AdminRejectionNote;

           
            await context.SaveChangesAsync(cancellationToken);

            // 3. إرسال الإشعار للبائع (Seller/Provider)
            //if (notificationService != null)
            //{
            //    // بافتراض أن الكلاس يمتلك ProviderProfileId أو SellerProfileId والذي يمثل الـ UserId
            //    await notificationService.SendAsync(
            //        userId: listing.ProviderProfileId,
            //        title: "تحديث بخصوص عرضك في السوق",
            //        message: $"تم رفض عرضك '{listing.Title}' لسبب: {request.AdminRejectionNote}. يرجى التعديل وإعادة التقديم."
            //    );
            //}

            return new RejectMarketplaceListingResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = listing.Id,
                Status = "Rejected",
                StatusAr = "مرفوض",
                AdminRejectionNote = listing.AdminRejectionNote
            };
        }
    }
}
