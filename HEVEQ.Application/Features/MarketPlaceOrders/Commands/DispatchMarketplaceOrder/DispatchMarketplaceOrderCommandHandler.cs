using AutoMapper;
using HEVEQ.Application.Common.Enums;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Helpers;
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
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder
{
    public class DispatchMarketplaceOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<DispatchMarketplaceOrderCommand, OrderActionResponse>
    {
        public async Task<OrderActionResponse> Handle(DispatchMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                .Include(o => o.Listing)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            if (order.Listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to dispatch this order.");

            if (order.Status != MarketplaceOrderStatus.SellerConfirmed)
                throw new ValidationException("Status",
                    $"Order cannot be dispatched from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.SellerConfirmed}'.");

            order.Status = MarketplaceOrderStatus.Dispatched;
            order.DispatchedAt = DateTime.UtcNow;
            order.TrackingNumber = string.IsNullOrWhiteSpace(request.Request.TrackingNumber)
                                        ? ReferenceNumberGenerator.Generate(ReferenceNumberType.TrackingNumber,order.Id)
                                        : request.Request.TrackingNumber;

            OrderNotifier.Notify(context, order.BuyerId, "OrderDispatched",
                 "Order dispatched", $"Your order {order.OrderNumber} has been dispatched.", order.Id);

            await context.SaveChangesAsync(cancellationToken);

            return new OrderActionResponse(
                order.Id, 
                order.Status.ToString(),
                order.Status.ToArabic(), 
                "Order dispatched successfully");
        }
    }
}
