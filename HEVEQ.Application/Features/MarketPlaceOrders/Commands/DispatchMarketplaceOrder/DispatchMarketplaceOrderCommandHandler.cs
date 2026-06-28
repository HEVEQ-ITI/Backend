using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
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
    public class DispatchMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<DispatchMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(DispatchMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
                .Include(o => o.Listing).ThenInclude(l => l.Seller)
                .Include(o => o.Buyer)
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
                                        ? TrackingNumberFormatter.Generate(order.Id, order.DispatchedAt.Value)
                                        : request.Request.TrackingNumber;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<MarketplaceOrderDto>(order);
        }
    }
}
