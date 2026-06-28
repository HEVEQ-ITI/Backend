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

namespace HEVEQ.Application.Features.Admin.Query.GetPendingMarketplaceListings
{
    public class GetPendingMarketplaceListingsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetPendingMarketplaceListingsQuery, PaginatedPendingMarketplaceResponse>
    {
        public async Task<PaginatedPendingMarketplaceResponse> Handle(GetPendingMarketplaceListingsQuery request, CancellationToken cancellationToken)
        {
            var query = context.MarketplaceListings
                .Include(m => m.Category)
                .Where(m => m.Status == MarketplaceListingStatus.PendingReview)
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedListings = await query
                .OrderBy(m => m.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var ownerIds = pagedListings.Select(m => m.SellerId).Distinct().ToList();

            var ownersDict = await userManager.Users
                .Where(u => ownerIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);

            var items = pagedListings.Select(listing => new PendingMarketplaceListingDto
            {
                Id = listing.Id,
                Title = listing.Title,
                OwnerName = ownersDict.ContainsKey(listing.SellerId) ? ownersDict[listing.SellerId] : "Unknown User",
                CategoryName = listing.Category?.Name ?? "Uncategorized",
                AiRiskScore = listing.AiRiskScore,
                AiRiskLevel = listing.AiRiskLevel.ToString(),
                SubmittedAt = listing.CreatedAt
            }).ToList();

            return new PaginatedPendingMarketplaceResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
