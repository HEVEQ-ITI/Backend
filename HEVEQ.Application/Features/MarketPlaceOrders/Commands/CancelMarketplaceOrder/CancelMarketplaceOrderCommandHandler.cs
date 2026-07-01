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

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder
{
    public class CancelMarketplaceOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<CancelMarketplaceOrderCommand, OrderActionResponse>
    {
        public async Task<OrderActionResponse> Handle(CancelMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);



            var userId = currentUser.UserId.Value;
            var isBuyer = order.BuyerId == userId;
            var isSeller = order.Listing.SellerId == userId;

            if (!isBuyer && !isSeller)
                throw new ForbiddenAccessException("You are not authorized to cancel this order.");

            if (order.Status == MarketplaceOrderStatus.Completed)
                throw new ValidationException("Status", "A completed order cannot be cancelled.");

            if (order.Status is MarketplaceOrderStatus.CancelledPreDispatch
                             or MarketplaceOrderStatus.CancelledPostDispatch)
                throw new ValidationException("Status", "This order has already been cancelled.");

            var isPostDispatch = order.Status is MarketplaceOrderStatus.Dispatched
                                          or MarketplaceOrderStatus.Delivered;

            order.Status = isPostDispatch
            ? MarketplaceOrderStatus.CancelledPostDispatch
            : MarketplaceOrderStatus.CancelledPreDispatch;

            order.CancellationReason = request.Request.Reason;
            order.CancelledAt = DateTime.UtcNow;
            order.CancellationInitiatedByRole = isBuyer
                ? MarketplaceOrderCancellationInitiator.Buyer
                : MarketplaceOrderCancellationInitiator.Seller;

            // Only reactivate the listing if cancelled before dispatch
            if (!isPostDispatch)
            {
                order.Listing.Status = MarketplaceListingStatus.Active;
                order.Listing.UpdatedAt = DateTime.UtcNow;
            }

            var notifyUserId = isBuyer ? order.Listing.SellerId : order.BuyerId;
            OrderNotifier.Notify(context, notifyUserId, "OrderCancelled",
                "Order cancelled", $"Order {order.OrderNumber} was cancelled.", order.Id);

            await context.SaveChangesAsync(cancellationToken);

            return new OrderActionResponse(
                order.Id,
                order.Status.ToString(),
                order.Status.ToArabic(), 
                "Order cancelled successfully");

        }
    }
}
