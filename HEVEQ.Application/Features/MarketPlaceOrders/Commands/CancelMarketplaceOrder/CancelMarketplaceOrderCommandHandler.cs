using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder
{
    public class CancelMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CancelMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(CancelMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
             .Include(o => o.Listing).ThenInclude(l => l.Seller)
             .Include(o => o.Buyer)
             .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
             ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

            var isBuyer = order.BuyerId == request.UserId;
            var isSeller = order.Listing.SellerId == request.UserId;

            if (!isBuyer && !isSeller)
                throw new UnauthorizedAccessException("You are not authorized to cancel this order.");

            if (order.Status == MarketplaceOrderStatus.Completed)
                throw new InvalidOperationException("A completed order cannot be cancelled.");

            if (order.Status is MarketplaceOrderStatus.CancelledPreDispatch
                             or MarketplaceOrderStatus.CancelledPostDispatch)
                throw new InvalidOperationException("This order has already been cancelled.");

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

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<MarketplaceOrderDto>(order);

        }
    }
}
