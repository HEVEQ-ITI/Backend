using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketPlaceOrderById
{
    public class GetMarketplaceOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMarketplaceOrderByIdQuery, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(GetMarketplaceOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
            .Include(o => o.Buyer)
            .Include(o => o.Listing).ThenInclude(l => l.Seller)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order is null)
                throw new InvalidOperationException("Marketplace order was not found.");

            var isBuyerOwner = order.BuyerId == request.UserId;
            var isSellerOwner = order.Listing.SellerId == request.UserId;

            if (!isBuyerOwner && !isSellerOwner)
                throw new InvalidOperationException("You are not allowed to view this order.");

            return mapper.Map<MarketplaceOrderDto>(order);
        }
    }
}
