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
                query = query.Where(l => l.Title.Contains(request.Query) || (l.Tags != null && l.Tags.Contains(request.Query)));
            }

            if (!string.IsNullOrWhiteSpace(request.Governorate))
            {
                query = query.Where(l => l.ProviderProfile.User.Addresses.Any(a => a.IsDefault && a.Governorate == request.Governorate));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(l => new PublicServiceListingDto(
                    l.Id,
                    l.Title,
                    l.Category.Name,
                    l.ProviderProfile.CompanyName,
                    l.Photos.OrderBy(p => p.Id).Select(p => p.PhotoUrl).FirstOrDefault() ?? "",
                    l.HourlyRate ?? 0,  // not worry i validate this in create listing validation will not be 0
                    l.DailyRate ?? 0,
                    l.MinimumBookingHours,
                    request.Governorate ?? "Cairo",
                    50,
                    4.5,
                    10,
                    "High",
                    "Available",
                    l.Status.ToString(),
                    l.Status.ToArabicText()
                ))
                .ToListAsync(cancellationToken);

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
