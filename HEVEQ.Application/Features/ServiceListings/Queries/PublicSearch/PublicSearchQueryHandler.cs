using HEVEQ.Application.Common.Extensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.ServiceListings.Queries.PublicSearch
{
    public class PublicSearchQueryHandler(IApplicationDbContext context)
        : IRequestHandler<PublicSearchQuery, PublicSearchResultDto>
    {
        public async Task<PublicSearchResultDto> Handle(PublicSearchQuery request, CancellationToken cancellationToken)
        {
            if (request.Context.Equals("marketplace", StringComparison.OrdinalIgnoreCase))
            {
                return new PublicSearchResultDto(
                    Query: request.Query,
                    Context: request.Context,
                    Items: [],
                    ResultCount: 0,
                    HasLowConfidence: true,
                    Message: "Marketplace search is owned by a different feature domain — pending cross-team contract."
                );
            }

            var query = context.ServiceListings
                .AsNoTracking()
                .Where(l => l.Status == ServiceListingStatus.Active);

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var searchKeyword = request.Query.Trim();

                query = query.Where(l => l.Title.Contains(searchKeyword)
                                      || (l.Description != null && l.Description.Contains(searchKeyword))
                                      || (l.Tags != null && l.Tags.Contains(searchKeyword)));
            }


            if (!string.IsNullOrWhiteSpace(request.Governorate))
            {
                query = query.Where(l => l.ProviderProfile.User.Addresses.Any(a => a.IsDefault && a.Governorate == request.Governorate));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var rawItems = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(l => new
            {
                l.Id,
                l.Title,
                CategoryName = l.Category != null ? l.Category.Name : "General",
                ProviderCompany = l.ProviderProfile != null ? l.ProviderProfile.CompanyName : "Unknown Provider",
                CoverPhotoUrl = l.Photos.OrderBy(p => p.DisplayOrder).Select(p => p.PhotoUrl).FirstOrDefault(),
                HourlyRate = l.HourlyRate ?? 0,
                DailyRate = l.DailyRate ?? 0,
                l.MinimumBookingHours,
                Location = (l.Region ?? "") + ", " + (l.Governorate ?? ""),
                ServiceRadiusKm = l.ProviderProfile != null ? l.ProviderProfile.ServiceRadiusKm : 0,
                AverageRating = l.ProviderProfile != null ? (double)l.ProviderProfile.AverageRating : 0.0,
                TotalReviewsCount = l.ProviderProfile != null ? l.ProviderProfile.TotalReviewsCount : 0,
                ProviderTrustLevel = l.ProviderProfile != null ? l.ProviderProfile.TrustLevel.ToString() : "Standard",
                l.Status
            })
            .ToListAsync(cancellationToken);

          
            var items = rawItems.Select(l => new PublicServiceListingDto(
                l.Id,
                l.Title,
                l.CategoryName,
                l.ProviderCompany,
                l.CoverPhotoUrl ?? "",
                l.HourlyRate,
                l.DailyRate,
                l.MinimumBookingHours,
                l.Location,
                l.ServiceRadiusKm,
                l.AverageRating,
                l.TotalReviewsCount,
                l.ProviderTrustLevel,
                "Available",
                l.Status.ToString(),
                l.Status.ToArabicText()
            )).ToList();


            return new PublicSearchResultDto(
            Query: request.Query,
            Context: request.Context,
            Items: items,
            ResultCount: totalCount,
            HasLowConfidence: false,
            Message: totalCount == 0 ? "No results found" : null
        );

        }
    }
}
