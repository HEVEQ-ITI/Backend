using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder
{
    public class CompleteMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CompleteMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(CompleteMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await context.MarketplaceOrders
             .Include(o => o.Listing).ThenInclude(l => l.Seller)
             .Include(o => o.Buyer)
             .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
             ?? throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

            if (order.BuyerId != request.BuyerId)
                throw new UnauthorizedAccessException("You are not authorized to complete this order.");

            if (order.Status != MarketplaceOrderStatus.Delivered)
                throw new InvalidOperationException(
                    $"Order cannot be completed from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.Delivered}'.");

            order.Status = MarketplaceOrderStatus.Completed;
            order.ConfirmedByBuyerAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<MarketplaceOrderDto>(order);
            
        }
    }
}
