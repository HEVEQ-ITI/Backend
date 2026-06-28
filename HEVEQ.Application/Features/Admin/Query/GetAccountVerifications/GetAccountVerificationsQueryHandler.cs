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

namespace HEVEQ.Application.Features.Admin.Query.GetAccountVerifications
{
    public class GetAccountVerificationsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetAccountVerificationsQuery, PaginatedVerificationsResponse>
    {
        public async Task<PaginatedVerificationsResponse> Handle(GetAccountVerificationsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Documents
                .Where(d => d.Status == DocumentVerificationStatus.Pending)
                .AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedDocuments = await query
                .OrderBy(d => d.UploadedAt) 
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            if (!pagedDocuments.Any())
            {
                return new PaginatedVerificationsResponse { TotalCount = totalCount };
            }

            var userIds = pagedDocuments.Select(d => d.UserId).Distinct().ToList();
            var users = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var userDict = new Dictionary<Guid, VerificationUserDto>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userDict[user.Id] = new VerificationUserDto
                {
                    Id = user.Id,
                    DisplayName = $"{user.FirstName} {user.LastName}".Trim(),
                    Role = roles.FirstOrDefault() ?? "User"
                };
            }

            var items = pagedDocuments.Select(doc => new PendingVerificationDto
            {
                DocumentId = doc.Id,
                DocumentType = doc.DocumentType.ToString(),
                FileUrl = doc.FileUrl,
                Status = doc.Status.ToString(),
                UploadedAt = doc.UploadedAt,
                User = userDict.GetValueOrDefault(doc.UserId!.Value)
            }).ToList();

            return new PaginatedVerificationsResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
