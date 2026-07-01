using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ApproveServiceListing
{
    public class ApproveServiceListingCommandHandler(IApplicationDbContext context)
        : IRequestHandler<ApproveServiceListingCommand, ApproveServiceListingResponse>
    {
        public async Task<ApproveServiceListingResponse> Handle(ApproveServiceListingCommand request, CancellationToken cancellationToken)
        {
            var listing = await context.ServiceListings.Include(x=>x.Availability)
                 .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (listing == null)
            {
                return new ApproveServiceListingResponse { IsSuccess = false, StatusCode = 404, Message = "Service listing not found." };
            }

            if (listing.Status != ServiceListingStatus.PendingReview)
            {
                return new ApproveServiceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Only listings in 'PendingReview' status can be approved."
                };
            }

            
            bool hasPhotos = listing.Photos != null && listing.Photos.Count > 0;
            bool hasOperatorInfo = listing.QsOperator> 0;
            bool hasAvailability = listing.Availability.Count() > 0;

            if (!hasPhotos || !hasOperatorInfo || !hasAvailability)
            {
                return new ApproveServiceListingResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Cannot approve listing. Minimum requirements are missing (Photos, Operator, or Availability details)."
                };
            }

            listing.Status = ServiceListingStatus.Active;

            await context.SaveChangesAsync(cancellationToken);

            // 6. إنشاء Notification للـ Provider لو الخدمة جاهزة (لو الـ Service محقونة وليست Null)
            //if (notificationService != null)
            //{
            //    await notificationService.SendAsync(
            //        userId: listing.ProviderId,
            //        title: "تم قبول خدمتك",
            //        message: $"تمت الموافقة على الخدمة '{listing.Title}' وهي الآن نشطة على المنصة."
            //    );
            //}

            // 7. إرجاع المخرجات المتوقعة بدقة
            return new ApproveServiceListingResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = listing.Id,
                Status = "Active",
                StatusText = "Active",
                StatusAr = "نشط",
                Message = "Service listing approved successfully"
            };
        }
    }
}
