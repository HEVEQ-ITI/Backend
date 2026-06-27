using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetCustomerOrder
{
    public class GetCustomerOrderQueryHandler(IApplicationDbContext context,IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<GetCustomerOrderQuery, List<PurchaseOrderDto>>
    {
        public async Task<List<PurchaseOrderDto>> Handle(GetCustomerOrderQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var orders = await context.MarketplaceOrders
                .AsNoTracking()
                .Include(o => o.Buyer)
                .Include(o => o.Listing).ThenInclude(l => l.Seller)
                .Where(o => o.BuyerId == currentUser.UserId.Value)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<PurchaseOrderDto>>(orders);
        }
    }
}
