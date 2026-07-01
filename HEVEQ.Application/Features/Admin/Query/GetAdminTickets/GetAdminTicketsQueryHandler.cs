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

namespace HEVEQ.Application.Features.Admin.Query.GetAdminTickets
{
    public class GetAdminTicketsQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetAdminTicketsQuery, PaginatedAdminTicketsResponse>
    {
        public async Task<PaginatedAdminTicketsResponse> Handle(GetAdminTicketsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Tickets.AsNoTracking();

            if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<TicketStatus>(request.Status, true, out var parsedStatus))
            {
                query = query.Where(t => t.Status == parsedStatus);
            }

            if (!string.IsNullOrEmpty(request.Category) && Enum.TryParse<TicketCategory>(request.Category, true, out var parsedCategory))
            {
                query = query.Where(t => t.Category == parsedCategory);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedData = await query
                .OrderByDescending(t => t.CreatedAt) 
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new
                {
                    t.Id,
                    t.TicketNumber,
                    t.Subject,
                    Category = t.Category.ToString(),
                    Status = t.Status.ToString(),
                    t.SubmittedById, 
                    t.CreatedAt
                })
                .ToListAsync(cancellationToken);

            if (!pagedData.Any())
            {
                return new PaginatedAdminTicketsResponse { TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize };
            }

            // 4. جلب أسماء المستخدمين من Identity
            var userIds = pagedData.Select(x => x.SubmittedById).Distinct().ToList();
            var usersDict = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);


            // 5. تجميع النتيجة النهائية وتعريب الحالة
            var items = pagedData.Select(t => new AdminTicketDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Subject = t.Subject,
                Category = t.Category,
                Status = t.Status,
                StatusAr = GetArabicStatus(t.Status),
                SubmittedByName = usersDict.GetValueOrDefault(t.SubmittedById, "Unknown User"),
                CreatedAt = t.CreatedAt
            }).ToList();

            return new PaginatedAdminTicketsResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        private static string GetArabicStatus(string status)
        {
            return status switch
            {
                "Open" => "مفتوحة",
                "InProgress" => "قيد المعالجة",
                "Resolved" => "محلولة",
                "Closed" => "مغلقة",
                _ => status
            };
        }
    }
}
