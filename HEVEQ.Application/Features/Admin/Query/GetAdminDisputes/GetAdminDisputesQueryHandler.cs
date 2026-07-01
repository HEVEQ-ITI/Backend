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

namespace HEVEQ.Application.Features.Admin.Query.GetAdminDisputes
{
    public class GetAdminDisputesQueryHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetAdminDisputesQuery, PaginatedAdminDisputesResponse>
    {
        public async Task<PaginatedAdminDisputesResponse> Handle(GetAdminDisputesQuery request, CancellationToken cancellationToken)
        {
            var disputedBookings = await context.Bookings
                .Where(b => b.Status == BookingStatus.Disputed) 
                .Select(b => new
                {
                    Id = b.Id,
                    Type = "Booking",
                    ReferenceNumber = b.BookingNumber, 
                    CustomerId = b.CustomerId,         
                    ProviderId = b.ServiceListing.ProviderProfileId,
                    Amount = b.EstimatedTotal, 
                    Status = b.Status.ToString(),
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var disputedOrders = await context.MarketplaceOrders
                .Where(o => o.Status == MarketplaceOrderStatus.Disputed) 
                .Select(o => new
                {
                    Id = o.Id,
                    Type = "MarketplaceOrder",
                    ReferenceNumber = o.Id.ToString().Substring(0, 8).ToUpper(),
                    CustomerId = o.BuyerId,            
                    ProviderId = o.Listing.SellerId,   
                    Amount = o.Amount,                
                    Status = o.Status.ToString(),
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var combinedDisputes = disputedBookings.Concat(disputedOrders)
                .OrderByDescending(d => d.CreatedAt)
                .ToList();

            var totalCount = combinedDisputes.Count;

            var pagedData = combinedDisputes
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            if (!pagedData.Any())
            {
                return new PaginatedAdminDisputesResponse { TotalCount = totalCount, Page = request.Page, PageSize = request.PageSize };
            }

            var userIds = pagedData.Select(d => d.CustomerId)
                .Concat(pagedData.Select(d => d.ProviderId))
                .Distinct()
                .ToList();

            var usersDict = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => $"{u.FirstName} {u.LastName}".Trim(), cancellationToken);

            var items = pagedData.Select(d => new AdminDisputeDto
            {
                Id = d.Id,
                Type = d.Type,
                ReferenceNumber = d.ReferenceNumber,
                CustomerName = usersDict.GetValueOrDefault(d.CustomerId, "Unknown Customer"),
                ProviderName = usersDict.GetValueOrDefault(d.ProviderId, "Unknown Provider"),
                Amount = d.Amount,
                Status = d.Status,
                StatusAr = d.Status == "Disputed" ? "متنازع عليه" : d.Status,
                CreatedAt = d.CreatedAt
            }).ToList();

            return new PaginatedAdminDisputesResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
