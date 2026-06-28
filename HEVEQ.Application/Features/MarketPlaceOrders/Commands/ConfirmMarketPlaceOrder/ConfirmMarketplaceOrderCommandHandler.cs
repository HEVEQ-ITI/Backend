using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
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
    public class ConfirmMarketplaceOrderCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<ConfirmMarketplaceOrderCommand, MarketplaceOrderDto>
    {
        public async Task<MarketplaceOrderDto> Handle(ConfirmMarketplaceOrderCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var order = await context.MarketplaceOrders
             .Include(o => o.Listing).ThenInclude(l => l.Seller)
             .Include(o => o.Buyer)
             .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
             ?? throw new NotFoundException(nameof(MarketplaceOrder), request.OrderId);

            if (order.Listing.SellerId != currentUser.UserId.Value)
                throw new ForbiddenAccessException("You are not authorized to confirm this order.");

            if (order.Status != MarketplaceOrderStatus.PaymentCaptured)
                throw new ValidationException("Status",
                    $"Order cannot be confirmed from its current status '{order.Status}'. Expected '{MarketplaceOrderStatus.PaymentCaptured}'.");

            order.Status = MarketplaceOrderStatus.SellerConfirmed;
            order.SellerConfirmedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<MarketplaceOrderDto>(order);
        }
    }
}
