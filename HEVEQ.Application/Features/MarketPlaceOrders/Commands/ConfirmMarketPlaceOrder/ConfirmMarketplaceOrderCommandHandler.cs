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

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketPlaceOrder
{
    public class ConfirmMarketplaceOrderCommandHandler(IApplicationDbContext context,  ICurrentUserService currentUser) : IRequestHandler<ConfirmMarketplaceOrderCommand, OrderActionResponse>
    {
        public async Task<OrderActionResponse> Handle(ConfirmMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
             .Include(o => o.Listing)
             .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
             ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            if (order.Listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to confirm this order.");

            if (order.Status != MarketplaceOrderStatus.PaymentCaptured)
                throw new ValidationException("Status",
                    $"Order cannot be confirmed from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.PaymentCaptured}'.");

            order.Status = MarketplaceOrderStatus.SellerConfirmed;
            order.SellerConfirmedAt = DateTime.UtcNow;

            OrderNotifier.Notify(context, order.BuyerId, "OrderSellerConfirmed",
                    "Order confirmed", $"Your order {order.OrderNumber} has been confirmed by the seller.", order.Id);

            await context.SaveChangesAsync(cancellationToken);

            return new OrderActionResponse(
                order.Id,
                order.Status.ToString(), 
                order.Status.ToArabic(), 
                "Order confirmed successfully");
        }
    }
}
