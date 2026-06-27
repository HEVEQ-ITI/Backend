using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketPlaceOrderById
{
    public class GetMarketplaceOrderByIdQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<GetMarketplaceOrderByIdQuery, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(GetMarketplaceOrderByIdQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
            .Include(o => o.Buyer)
            .Include(o => o.Listing).ThenInclude(l => l.Seller)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            var userId = currentUser.UserId.Value;
            var isBuyerOwner = order.BuyerId == userId;
            var isSellerOwner = order.Listing.SellerId == userId;

            if (!isBuyerOwner && !isSellerOwner)
                throw new ForbiddenAccessException("You are not allowed to view this order.");

            var dto = mapper.Map<MarketplaceOrderDto>(order);
            dto.ViewerRole = isBuyerOwner ? "Buyer" : "Seller";
            return dto;
        }
    }
}
