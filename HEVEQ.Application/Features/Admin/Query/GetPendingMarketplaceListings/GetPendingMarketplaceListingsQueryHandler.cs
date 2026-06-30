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
                 .Where(m => m.Status == MarketplaceListingStatus.PendingReview) // حسب المُسمى لديك
                 .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedData = await query
                .OrderBy(m => m.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.SellerId, 
                    CategoryName = m.Category != null ? m.Category.Name : "Uncategorized",
                    m.Price,
                    PhotosCount = m.Photos.Count(), 

                    m.AiRiskScore,
                    m.AiRiskLevel,
                    m.AiRiskFlags,
                    m.CreatedAt
                })
                .ToListAsync(cancellationToken);

            if (!pagedData.Any())
            {
                return new PaginatedPendingMarketplaceResponse { TotalCount = totalCount };
            }

            var userIds = pagedData.Select(x => x.SellerId).Distinct().ToList();

            var usersDict = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);

            var items = pagedData.Select(x => new PendingMarketplaceListingDto
            {
                Id = x.Id,
                Title = x.Title,
                OwnerName = usersDict.GetValueOrDefault(x.SellerId, "Unknown Owner"),
                CategoryName = x.CategoryName,
                Price = x.Price,
                PhotosCount = x.PhotosCount,
                AiRiskScore = x.AiRiskScore,
                AiRiskLevel = x.AiRiskLevel ?? "N/A",
                AiRiskFlags = !string.IsNullOrEmpty(x.AiRiskFlags) ? x.AiRiskFlags : "[]",
                Status = "PendingReview",
                StatusAr = "قيد المراجعة",
                SubmittedAt = x.CreatedAt
            }).ToList();

            return new PaginatedPendingMarketplaceResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
