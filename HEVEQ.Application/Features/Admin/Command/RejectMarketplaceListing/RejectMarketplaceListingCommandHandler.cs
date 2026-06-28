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
            // 1. جلب العنصر المستهدف من جدول السوق
            var listing = await context.MarketplaceListings
                .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new RejectMarketplaceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Marketplace listing not found." };
            }

            // 2. التأكد من حالة العنصر (يجب أن يكون في انتظار المراجعة)
            if (listing.Status != MarketplaceListingStatus.PendingReview)
            {
                return new RejectMarketplaceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot reject listing. Current status is {listing.Status}."
                };
            }

            // 3. تحديث الحالة وإضافة سبب الرفض
            listing.Status = MarketplaceListingStatus.Rejected;
            listing.AdminRejectionNote = request.AdminRejectionNote;

            // 4. حفظ التغييرات في قاعدة البيانات
            await context.SaveChangesAsync(cancellationToken);

            // 5. تجهيز وإرجاع النتيجة
            return new RejectMarketplaceListingResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = listing.Id,
                Status = "Rejected", // أو listing.ApprovalStatus.ToString()
                StatusAr = "مرفوض",
                AdminRejectionNote = listing.AdminRejectionNote
            };
        }
    }
}
