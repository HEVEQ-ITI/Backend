using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetMarketplaceListingReviewDetails
{
    public class GetMarketplaceListingReviewDetailsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetMarketplaceListingReviewDetailsQuery, MarketplaceListingReviewDetailsDto>
    {
        public async Task<MarketplaceListingReviewDetailsDto> Handle(GetMarketplaceListingReviewDetailsQuery request, CancellationToken cancellationToken)
        {
            var listingData = await context.MarketplaceListings
                .Where(m => m.Id == request.Id)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Description,
                    m.SellerId,
                    CategoryName = m.Category != null ? m.Category.Name : "Uncategorized",
                    m.Price,
                    Condition = m.Condition != null ? m.Condition.ToString() : "Unknown",

                   
                    Photos = m.Photos.Select(p => new ReviewPhotoDto { Id = p.Id, Url = p.PhotoUrl ?? "" }).ToList(),
                    Documents = context.Documents
                        .Where(d => d.DocumentType == DocumentType.EquipmentLicense && d.MarketplaceListingId == m.Id)
                        .Select(d => new ReviewDocumentDto { Id = d.Id, Type = d.DocumentType.ToString(), FileUrl = d.FileUrl })
                        .ToList(),

                    m.AiRiskScore,
                    m.AiRiskLevel,
                    m.AiRiskFlags,
                    Status = m.Status.ToString()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (listingData == null)
            {
                return null;
            }

            var sellerUser = await userManager.FindByIdAsync(listingData.SellerId.ToString());

            var sellerInfo = new ReviewSellerDto
            {
                Id = listingData.SellerId,
                DisplayName = sellerUser != null ? $"{sellerUser.FirstName} {sellerUser.LastName}".Trim() : "Unknown Seller",
                Email = sellerUser?.Email ?? "N/A"
            };

            string conditionAr = listingData.Condition switch
            {
                "New" => "جديد",
                "Excellent" => "ممتاز",
                "Good" => "جيد",
                "Fair" => "مقبول",
                "Poor" => "ضعيف",
                _ => "غير محدد"
            };

            string statusAr = listingData.Status switch
            {
                "PendingReview" => "قيد المراجعة",
                "Active" => "نشط",
                "Rejected" => "مرفوض",
                _ => listingData.Status
            };

            return new MarketplaceListingReviewDetailsDto
            {
                Id = listingData.Id,
                Title = listingData.Title,
                Description = listingData.Description,
                CategoryName = listingData.CategoryName,

                Seller = sellerInfo, 

                Price = listingData.Price,
                Condition = listingData.Condition,
                ConditionAr = conditionAr,

                Photos = listingData.Photos,
                Documents = listingData.Documents,

                AiRiskScore = listingData.AiRiskScore,
                AiRiskLevel = listingData.AiRiskLevel ?? "N/A",
                AiRiskFlags = !string.IsNullOrEmpty(listingData.AiRiskFlags) ? listingData.AiRiskFlags : "[]",

                Status = listingData.Status,
                StatusAr = statusAr
            };
        }
    }
}
