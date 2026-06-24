using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder
{
    public class DeliverMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<DeliverMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(DeliverMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
             .Include(o => o.Listing).ThenInclude(l => l.Seller)
             .Include(o => o.Buyer)
             .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
             ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

            if (order.Listing.SellerId != request.SellerId)
                throw new UnauthorizedAccessException("You are not authorized to confirm this order.");

            if (order.Status != MarketplaceOrderStatus.Dispatched)
                throw new InvalidOperationException(
                    $"Order cannot be marked as delivered from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.Dispatched}'.");

            order.Status = MarketplaceOrderStatus.Delivered;
            order.DeliveredAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<MarketplaceOrderDto>(order);

        }
    }
}
