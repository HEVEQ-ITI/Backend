using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HEVEQ.Domain.Enums;
using HEVEQ.Application.Features.Admin.DTOs;

namespace HEVEQ.Application.Features.Admin.Query.GetPendingServiceListings
{
    public class GetPendingServiceListingsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetPendingServiceListingsQuery, PaginatedPendingListingsResponse>
    {
        public async Task<PaginatedPendingListingsResponse> Handle(GetPendingServiceListingsQuery request, CancellationToken cancellationToken)
        {
            var query = context.ServiceListings
                .Include(s => s.Category)
                .Where(s => s.Status == ServiceListingStatus.PendingReview) 
                .AsNoTracking();

            
            var totalCount = await query.CountAsync(cancellationToken);

            var pagedListings = await query
                .OrderBy(s => s.CreatedAt) 
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var providerIds = pagedListings.Select(s => s.ProviderProfileId).Distinct().ToList();

            var providersDict = await userManager.Users
                .Where(u => providerIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);

            var items = pagedListings.Select(listing => new PendingServiceListingDto
            {
                Id = listing.Id,
                Title = listing.Title,
                OwnerName = providersDict.ContainsKey(listing.ProviderProfileId) ? providersDict[listing.ProviderProfileId] : "Unknown",
                CategoryName = listing.Category?.Name ?? "Uncategorized",
                AiRiskScore = listing.AiRiskScore,
                AiRiskLevel = listing.AiRiskLevel.ToString(), 
                QualityScore = listing.QualityScore,
                SubmittedAt = listing.CreatedAt
            }).ToList();

            return new PaginatedPendingListingsResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
