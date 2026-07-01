using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Localization;
using HEVEQ.Application.Features.MarketPlaceOrders.Common;
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
    public class DeliverMarketplaceOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<DeliverMarketplaceOrderCommand, OrderActionResponse>
    {
        public async Task<OrderActionResponse> Handle(DeliverMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            if (order.Listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to mark this order as delivered.");

            if (order.Status != MarketplaceOrderStatus.Dispatched)
                throw new ValidationException("Status",
                    $"Order cannot be marked as delivered from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.Dispatched}'.");

            order.Status = MarketplaceOrderStatus.Delivered;
            order.DeliveredAt = DateTime.UtcNow;

            OrderNotifier.Notify(context, order.BuyerId, "OrderDelivered",
                 "Order delivered", $"Your order {order.OrderNumber} has been marked as delivered.", order.Id);

            await context.SaveChangesAsync(cancellationToken);

            return new OrderActionResponse(
                order.Id,
                order.Status.ToString(),
                order.Status.ToArabic(), 
                "Order marked as delivered successfully");
        }
    }
}
