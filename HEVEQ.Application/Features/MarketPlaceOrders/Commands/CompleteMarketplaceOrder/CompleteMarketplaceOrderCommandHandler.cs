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

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder
{
    public class CompleteMarketplaceOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<CompleteMarketplaceOrderCommand, OrderActionResponse>
    {
        public async Task<OrderActionResponse> Handle(CompleteMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                 .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            if (order.BuyerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to complete this order.");

            if (order.Status != MarketplaceOrderStatus.Delivered)
                throw new ValidationException("Status",
                    $"Order cannot be completed from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.Delivered}'.");

            order.Status = MarketplaceOrderStatus.Completed;
            order.ConfirmedByBuyerAt = DateTime.UtcNow;

            OrderNotifier.Notify(context, order.Listing.SellerId, "OrderCompleted",
                "Order completed", $"Order {order.OrderNumber} has been marked complete by the buyer.", order.Id);


            await context.SaveChangesAsync(cancellationToken);

            return new OrderActionResponse(
                order.Id,
                order.Status.ToString(),
                order.Status.ToArabic(),
                "Order completed successfully");

        }
    }
}
