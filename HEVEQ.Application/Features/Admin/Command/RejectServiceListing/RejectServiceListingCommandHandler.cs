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
            var listing = await context.ServiceListings
                .Include(s => s.ProviderProfile)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new RejectServiceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Service listing not found." };
            }

            if (listing.Status != ServiceListingStatus.PendingReview)
            {
                return new RejectServiceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Cannot reject listing. Current status is {listing.Status}."
                };
            }

            listing.Status = ServiceListingStatus.Rejected;
            listing.AdminRejectionNote = request.AdminRejectionNote;
            listing.RejectedByAdminId = request.AdminId; 
            listing.RejectedAt = DateTime.UtcNow;       

            await context.SaveChangesAsync(cancellationToken);

            // 5. إرسال الإشعار للمزود (Provider) إذا كانت الخدمة متاحة
            //if (notificationService != null && listing.ProviderProfile != null)
            //{
            //    await notificationService.SendAsync(
            //        userId: listing.ProviderProfile.UserId,
            //        title: "تحديث بخصوص خدمتك",
            //        message: $"تم رفض خدمتك '{listing.Title}' لسبب: {request.AdminRejectionNote}. يرجى التعديل وإعادة التقديم."
            //    );
            //}

            return new RejectServiceListingResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = listing.Id,
                Status = "Rejected",
                StatusText = "Rejected",
                StatusAr = "مرفوض",
                AdminRejectionNote = listing.AdminRejectionNote
            };
        }
    }
}
