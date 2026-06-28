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
            // 1. جلب الخدمة
            var listing = await context.ServiceListings
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
                    Message = $"Cannot approve listing. Current status is {listing.Status}."
                };
            }

            listing.Status = ServiceListingStatus.Active;

            listing.EmbeddingStatus = EmbeddingStatus.Pending;

            await context.SaveChangesAsync(cancellationToken);

            return new ApproveServiceListingResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Id = listing.Id,
                Status = "Active", 
                StatusAr = "نشط",
                Message = "Service listing approved successfully"
            };
        }
    }
}
