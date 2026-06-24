using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder
{
    public class DispatchMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<DispatchMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(DispatchMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
              .Include(o => o.Listing).ThenInclude(l => l.Seller)
              .Include(o => o.Buyer)
              .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
              ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

            if (order.Listing.SellerId != request.SellerId)
                throw new UnauthorizedAccessException("You are not authorized to confirm this order.");

            if (order.Status != MarketplaceOrderStatus.SellerConfirmed)
                throw new InvalidOperationException(
                    $"Order cannot be dispatched from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.SellerConfirmed}'.");
            order.Status = MarketplaceOrderStatus.Dispatched;
            order.DispatchedAt = DateTime.UtcNow;
            order.TrackingNumber = request.Request.TrackingNumber;
            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<MarketplaceOrderDto>(order);
        }
    }
}
