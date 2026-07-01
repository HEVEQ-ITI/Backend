using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.Documents.DTOs;
using HEVEQ.Application.Features.Documents.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Queries.GetAdminDocuments
{
    public class GetAdminDocumentsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAdminDocumentsQuery, AdminPagedResult<AdminDocumentDto>>
    {
        public async Task<AdminPagedResult<AdminDocumentDto>> Handle(GetAdminDocumentsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Documents
                 .AsNoTracking()
                 .Include(d => d.User)
                 .Include(d => d.ServiceListing)
                 .Include(d => d.MarketplaceListing)
                 .Include(d => d.Operator)
                 .AsQueryable();

            // Status filter
            if (request.Status.HasValue)
                query = query.Where(d => d.Status == request.Status.Value);

            // DocumentType filter
            if (request.DocumentType.HasValue)
                query = query.Where(d => d.DocumentType == request.DocumentType.Value);

            // Role filter — narrows query only, doesn't own the execution path
            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                query = request.Role.ToLower() switch
                {
                    "provider" => query.Where(d => d.UserId.HasValue &&
                        context.ProviderProfiles.Any(p => p.UserId == d.UserId.Value)),
                    "customer" => query.Where(d => d.UserId.HasValue &&
                        context.CustomerProfiles.Any(c => c.UserId == d.UserId.Value)),
                    "employee" => query.Where(d => d.UserId.HasValue &&
                        context.EmployeeProfiles.Any(e => e.UserId == d.UserId.Value)),
                    _ => query
                };
            }

            query = query.OrderByDescending(d => d.UploadedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var documents = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var userIds = documents
                .Where(d => d.UserId.HasValue)
                .Select(d => d.UserId!.Value)
                .Distinct()
                .ToList();

            var providerUserIds = await context.ProviderProfiles
                .Where(p => userIds.Contains(p.UserId))
                .Select(p => p.UserId)
                .ToHashSetAsync(cancellationToken);

            var customerUserIds = await context.CustomerProfiles
                .Where(c => userIds.Contains(c.UserId))
                .Select(c => c.UserId)
                .ToHashSetAsync(cancellationToken);

            var employeeUserIds = await context.EmployeeProfiles
                .Where(e => userIds.Contains(e.UserId))
                .Select(e => e.UserId)
                .ToHashSetAsync(cancellationToken);

            string ResolveRole(Guid? userId)
            {
                if (!userId.HasValue) return "Unknown";
                if (providerUserIds.Contains(userId.Value)) return "Provider";
                if (customerUserIds.Contains(userId.Value)) return "Customer";
                if (employeeUserIds.Contains(userId.Value)) return "Employee";
                return "Unknown";
            }

            var items = documents.Select(d => new AdminDocumentDto
            {
                DocumentId = d.Id,
                DocumentType = d.DocumentType.ToString(),
                FileUrl = d.FileUrl,
                Status = d.Status.ToString(),
                StatusAr = d.Status.ToArabic(),
                UploadedAt = d.UploadedAt,
                ExpiryDate = d.ExpiryDate,
                ExpiryStatus = d.ExpiryStatus?.ToString(),
                ExtractedText = d.ExtractedText,
                ConfidenceScore = d.ConfidenceScore,
                KeyFieldsPresent = d.KeyFieldsPresent,
                User = d.User is null ? null : new AdminDocumentUserDto
                {
                    Id = d.User.Id,
                    DisplayName = !string.IsNullOrEmpty(d.User.FirstName)
                        ? $"{d.User.FirstName} {d.User.LastName}"
                        : d.User.UserName ?? string.Empty,
                    Role = ResolveRole(d.UserId)
                },
                LinkedEntity = DocumentMappingHelper.ResolveLinkedEntity(d)
            }).ToList();

            return new AdminPagedResult<AdminDocumentDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };


        }

    }
}
