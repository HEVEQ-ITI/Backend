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
                .Include(c=>c.Category)
                .Where(s => s.Status == ServiceListingStatus.PendingReview) 
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedData = await query
                .OrderBy(s => s.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.ProviderProfileId, 
                    CategoryName = s.Category != null ? s.Category.Name : "Uncategorized",

                    PhotosCount = s.Photos.Count(),
                    OperatorsCount = s.ServiceListingOperators.Count(),
                    DocumentsCount = s.Documents.Count(),

                    s.QualityScore,
                    s.AiRiskScore,
                    s.AiRiskLevel,
                    s.AiRiskFlags,
                    s.AiRecommendation,
                    s.CreatedAt
                })
                .ToListAsync(cancellationToken);

            if (!pagedData.Any())
            {
                return new PaginatedPendingListingsResponse { TotalCount = totalCount };
            }

            var profileIds = pagedData.Select(x => x.ProviderProfileId).Distinct().ToList();
            var profilesWithUserIds = await context.ProviderProfiles
                .Where(p => profileIds.Contains(p.Id))
                .Select(p => new { ProfileId = p.Id, UserId = p.UserId }) 
                .ToListAsync(cancellationToken);

            var userIds = profilesWithUserIds.Select(p => p.UserId).Distinct().ToList();
            var usersDict = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);

            var profileToOwnerNameDict = new Dictionary<Guid, string>();
            foreach (var profile in profilesWithUserIds)
            {
                if (usersDict.TryGetValue(profile.UserId, out var ownerName))
                {
                    profileToOwnerNameDict[profile.ProfileId] = ownerName;
                }
            }

            var items = pagedData.Select(x => new PendingServiceListingDto
            {
                Id = x.Id,
                Title = x.Title,
                OwnerName = profileToOwnerNameDict.GetValueOrDefault(x.ProviderProfileId, "Unknown Owner"),
                CategoryName = x.CategoryName,
                PhotosCount = x.PhotosCount,
                OperatorsCount = x.OperatorsCount,
                DocumentsCount = x.DocumentsCount,
                QualityScore = x.QualityScore ?? 0,
                AiRiskScore = x.AiRiskScore,
                AiRiskLevel = x.AiRiskLevel ?? "N/A",
                AiRiskFlags = !string.IsNullOrEmpty(x.AiRiskFlags) ? x.AiRiskFlags : "[]",
                AiRecommendation = x.AiRecommendation ?? "Pending AI Analysis",
                Status = "PendingReview",
                StatusAr = "قيد المراجعة",
                SubmittedAt = x.CreatedAt
            }).ToList();

            return new PaginatedPendingListingsResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
