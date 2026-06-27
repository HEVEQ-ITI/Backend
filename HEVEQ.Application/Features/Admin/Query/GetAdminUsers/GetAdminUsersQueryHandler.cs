using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Admin.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetAdminUsers
{
    public class GetAdminUsersQueryHandler(IApplicationDbContext context, UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetAdminUsersQuery, PaginatedUsersResponse>
    {
        public async Task<PaginatedUsersResponse> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
        {
            var query = userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(u =>
                    u.FirstName.ToLower().Contains(search) ||
                    u.LastName.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.PhoneNumber.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                bool isActive = request.Status.Equals("Active", StringComparison.OrdinalIgnoreCase);
                query = query.Where(u => u.IsActive == isActive);
            }

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(request.Role);
                var userIdsInRole = usersInRole.Select(u => u.Id).ToList();

                query = query.Where(u => userIdsInRole.Contains(u.Id));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedUsers = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var pagedUserIds = pagedUsers.Select(u => u.Id).ToList();

            var providerProfiles = await context.ProviderProfiles
                .Where(p => pagedUserIds.Contains(p.UserId))
                .ToDictionaryAsync(p => p.UserId, p => p.TrustScore, cancellationToken);

            var items = new List<UserAdminDTO>();

            foreach (var user in pagedUsers)
            {
                var roles = await userManager.GetRolesAsync(user);

                items.Add(new UserAdminDTO
                {
                    Id = user.Id,
                    DisplayName = $"{user.FirstName} {user.LastName}".Trim(),
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles.FirstOrDefault() ?? "User",
                    IsActive = user.IsActive,
                    TrustScore = providerProfiles.ContainsKey(user.Id) ? providerProfiles[user.Id] : null,
                   
                });
            }

            return new PaginatedUsersResponse
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
    
}
