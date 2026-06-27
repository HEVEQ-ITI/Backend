using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RejectServiceListing
{
    public class RejectServiceListingCommandHandler(IApplicationDbContext context)
        : IRequestHandler<RejectServiceListingCommand, RejectServiceListingResponse>
    {
        public async Task<RejectServiceListingResponse> Handle(RejectServiceListingCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب الخدمة المستهدفة
            var listing = await context.ServiceListings
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new RejectServiceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Service listing not found." };
            }

            // 2. التأكد من حالة الخدمة
            if (listing.Status != ServiceListingStatus.PendingReview)
            {
                return new RejectServiceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot reject listing. Current status is {listing.Status}."
                };
            }

            // 3. تحديث الحالة وإضافة سبب الرفض
            listing.Status = ServiceListingStatus.Rejected;
            listing.AdminRejectionNote = request.AdminRejectionNote;

            // 4. حفظ التغييرات
            await context.SaveChangesAsync(cancellationToken);

            // 5. إرجاع النتيجة
            return new RejectServiceListingResponse
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
