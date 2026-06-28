using HEVEQ.Application.Common.Extensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.ServiceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListings;

public class GetPublicServiceListingsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetPublicServiceListingsQuery, PublicPaginatedList<PublicServiceListingDto>>
{
    public async Task<PublicPaginatedList<PublicServiceListingDto>> Handle(
        GetPublicServiceListingsQuery request, CancellationToken cancellationToken)
    {
        // filter active services
        var query = context.ServiceListings
            .AsNoTracking()
            .Where(l => l.Status == ServiceListingStatus.Active);

        // filter by search term
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(l => (l.Title != null && l.Title.Contains(request.Search))
                                  || (l.Description != null && l.Description.Contains(request.Search)));
        }

        // filter by category
        if (request.CategoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == request.CategoryId.Value);
        }

        // filter by governorate
        if (!string.IsNullOrWhiteSpace(request.Governorate))
        {
            query = query.Where(l => l.Governorate != null && l.Governorate.ToLower() == request.Governorate.ToLower());
        }

        // filter by rates
        if (request.MinRate.HasValue)
            query = query.Where(l => l.HourlyRate >= request.MinRate.Value);

        if (request.MaxRate.HasValue)
            query = query.Where(l => l.HourlyRate <= request.MaxRate.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        // professional projection to anonymous object
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
                CoverPhotoUrl = l.Photos
                    .OrderBy(p => p.DisplayOrder)
                    .Select(p => p.PhotoUrl)
                    .FirstOrDefault(),
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

        // final mapping to dto using arabic text extension
        var items = rawItems.Select(l => new PublicServiceListingDto(
            l.Id,
            l.Title,
            l.CategoryName,
            l.ProviderCompany,
            l.CoverPhotoUrl,
            l.HourlyRate,
            l.DailyRate,
            l.MinimumBookingHours,
            l.Location,
            l.ServiceRadiusKm,
            l.AverageRating,
            l.TotalReviewsCount,
            l.ProviderTrustLevel,
            "Available this week",
            l.Status.ToString(),
            l.Status.ToArabicText()
        )).ToList();

        return new PublicPaginatedList<PublicServiceListingDto>(items, request.Page, request.PageSize, totalCount);
    }
}