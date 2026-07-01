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

namespace HEVEQ.Application.Features.Admin.Query.GetServiceListingReviewDetails
{
    public class GetServiceListingReviewDetailsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetServiceListingReviewDetailsQuery, ServiceListingReviewDetailsDto>
    {
        public async Task<ServiceListingReviewDetailsDto> Handle(GetServiceListingReviewDetailsQuery request, CancellationToken cancellationToken)
        {
            var listingData = await context.ServiceListings
                .Where(s => s.Id == request.Id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.ProviderProfileId, 
                    CategoryName = s.Category != null ? s.Category.Name : "Uncategorized",

                   
                    s.HourlyRate,
                    s.DailyRate,
                    s.MinimumBookingHours,

                    Photos = s.Photos.Select(p => new ReviewPhotoDto { Id = p.Id, Url = p.PhotoUrl ?? "" }).ToList(),
                    Operators = s.ServiceListingOperators.Select(o => new ReviewOperatorDto { Id = o.OperatorId, Name = o.Operator.FullName ?? "Unknown" }).ToList(),
                    Availability = s.Availability.Select(a => new ReviewAvailabilityDto { Day = a.DayOfWeek.ToString(), Hours = "08:00 - 17:00" }).ToList(),
                    Documents = s.Documents.Select(d => new ReviewDocumentDto { Id = d.Id, Type = d.DocumentType.ToString(), FileUrl = d.FileUrl }).ToList(),

                    
                    s.QualityScore,
                    s.AiRiskScore,
                    s.AiRiskLevel,
                    s.AiRiskFlags,
                    s.AiRecommendation,
                    Status = s.Status.ToString()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (listingData == null)
            {
                return null; 
            }

            var providerUser = await userManager.FindByIdAsync(listingData.ProviderProfileId.ToString());

            var providerInfo = new ReviewProviderDto
            {
                CompanyName = providerUser != null ? $"{providerUser.FirstName} {providerUser.LastName}".Trim() : "Unknown Provider",
                Email = providerUser?.Email ?? "N/A",
                PhoneNumber = providerUser?.PhoneNumber ?? "N/A"
            };

            return new ServiceListingReviewDetailsDto
            {
                Id = listingData.Id,
                Title = listingData.Title,
                Description = listingData.Description,
                CategoryName = listingData.CategoryName,

                Provider = providerInfo,

                Pricing = new ReviewPricingDto
                {
                    HourlyRate = listingData.HourlyRate,
                    DailyRate = listingData.DailyRate,
                    MinimumBookingHours = listingData.MinimumBookingHours
                },

                Photos = listingData.Photos,
                Operators = listingData.Operators,
                Availability = listingData.Availability,
                Documents = listingData.Documents,

                QualityScore = listingData.QualityScore ?? 0,
                AiRiskScore = listingData.AiRiskScore,
                AiRiskLevel = listingData.AiRiskLevel ?? "N/A",
                AiRiskFlags = !string.IsNullOrEmpty(listingData.AiRiskFlags) ? listingData.AiRiskFlags : "[]",
                AiRecommendation = listingData.AiRecommendation ?? "Pending AI Analysis",

                Status = listingData.Status,
                StatusAr = listingData.Status == ServiceListingStatus.PendingReview.ToString() ? "قيد المراجعة" :
                           listingData.Status == ServiceListingStatus.Active.ToString() ? "نشط" : "مرفوض"
            };
        }
    }

}
