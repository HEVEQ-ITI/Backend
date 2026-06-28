using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder
{
    public class DeliverMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<DeliverMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(DeliverMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                .Include(o => o.Listing).ThenInclude(l => l.Seller)
                .Include(o => o.Buyer)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            if (order.Listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to mark this order as delivered.");

            if (order.Status != MarketplaceOrderStatus.Dispatched)
                throw new ValidationException("Status",
                    $"Order cannot be marked as delivered from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.Dispatched}'.");

            order.Status = MarketplaceOrderStatus.Delivered;
            order.DeliveredAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<MarketplaceOrderDto>(order);

        }
    }
}
