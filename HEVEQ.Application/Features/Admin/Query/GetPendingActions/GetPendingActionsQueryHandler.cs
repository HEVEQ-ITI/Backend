using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetPendingActions
{
    public class GetPendingActionsQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetPendingActionsQuery, PaginatedPendingActionsResponse>
    {
        public async Task<PaginatedPendingActionsResponse> Handle(GetPendingActionsQuery request, CancellationToken cancellationToken)
        {
            var allPendingActions = new List<PendingActionDto>();

            var serviceActions = await GetPendingServicesAsync(cancellationToken);
            allPendingActions.AddRange(serviceActions);

            var marketplaceActions = await GetPendingMarketplaceListingsAsync(cancellationToken);
            allPendingActions.AddRange(marketplaceActions);

            var documentActions = await GetPendingDocumentsAsync(cancellationToken);
            allPendingActions.AddRange(documentActions);

            var TicketActions = await GetPendingTicketssAsync(cancellationToken);
            allPendingActions.AddRange(TicketActions);

            

            var totalCount = allPendingActions.Count;

            var pagedItems = allPendingActions
                .OrderBy(a => a.Priority == "High" ? 0 : 1) 
                .ThenBy(a => a.CreatedAt)                  
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PaginatedPendingActionsResponse
            {
                Items = pagedItems,
                TotalCount = totalCount
            };
        }


        private async Task<List<PendingActionDto>> GetPendingServicesAsync(CancellationToken cancellationToken)
        {
            return await context.ServiceListings
                .Where(s => s.Status == ServiceListingStatus.PendingReview)
                .Select(s => new PendingActionDto
                {
                    Type = "ServiceListingReview",
                    Title = s.Title,
                    ReferenceId = s.Id,
                    Priority = s.AiRiskScore >= 70 ? "High" : "Normal",
                    CreatedAt = s.CreatedAt,
                    ActionUrl = $"/admin/service-listings/{s.Id}/review"
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<List<PendingActionDto>> GetPendingMarketplaceListingsAsync(CancellationToken cancellationToken)
        {
            return await context.MarketplaceListings
                .Where(m => m.Status == MarketplaceListingStatus.PendingReview)
                .Select(m => new PendingActionDto
                {
                    Type = "MarketplaceListingReview",
                    Title = m.Title,
                    ReferenceId = m.Id,
                    Priority = m.AiRiskScore >= 70 ? "High" : "Normal",
                    CreatedAt = m.CreatedAt,
                    ActionUrl = $"/admin/marketplace-listings/{m.Id}/review"
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<List<PendingActionDto>> GetPendingDocumentsAsync(CancellationToken cancellationToken)
        {
            return await context.Documents
                .Where(d => d.Status == DocumentVerificationStatus.Pending)
                .Select(d => new PendingActionDto
                {
                    Type = "DocumentVerification",
                    Title = $"وثيقة تحقيق شخصية - رقم حساب: {d.UserId}", 
                    ReferenceId = d.Id,
                    Priority = "Normal",
                    CreatedAt = d.UploadedAt,
                    ActionUrl = "/admin/account-verifications"
                })
                .ToListAsync(cancellationToken);
        }
        private async Task<List<PendingActionDto>> GetPendingTicketssAsync(CancellationToken cancellationToken)
        {
            return await context.Tickets
                .Where(d => d.Status == TicketStatus.Open ||
                d.Status == TicketStatus.PendingFieldVerification ||
                d.Status == TicketStatus.PendingCustomerReply || 
                d.Status == TicketStatus.PendingProviderReply)
                .Select(d => new PendingActionDto
                {
                    Type = "TicketVerification",
                    Title = $"تذكرة مفتوحة - رقم التذكرة {d.TicketNumber}",
                    ReferenceId = d.Id,
                    Priority = d.Priority >= 70 ? "High" : "Normal",
                    CreatedAt = d.CreatedAt,
                    ActionUrl = "/admin/account-verifications"
                })
                .ToListAsync(cancellationToken);
        }
    }
}
