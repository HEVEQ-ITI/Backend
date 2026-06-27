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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder
{
    public class CreateMarketPlaceOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser) : IRequestHandler<CreateMarketPlaceOrderCommand, CreateMarketplaceOrderResponse>
    {
        
        public async Task<CreateMarketplaceOrderResponse> Handle(CreateMarketPlaceOrderCommand command, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("User is not authenticated.");

            var buyerId = currentUser.UserId.Value;
            var request = command.Request;

            var listing = await context.MarketplaceListings
                .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken)
                ?? throw new NotFoundException(nameof(MarketplaceListing), request.ListingId);

            if (listing.Status != MarketplaceListingStatus.Active)
                throw new ValidationException("ListingId", "Orders can only be placed on approved, active listings.");

            if (listing.SellerId == buyerId)
                throw new ForbiddenAccessException("You cannot purchase your own listing.");

            var order = new MarketplaceOrder
            {
                BuyerId = buyerId,
                ListingId = listing.Id,
                Amount = listing.Price, // snapshot at time of purchase
                DeliveryAddress = request.DeliveryAddress,
                DeliveryPreference = request.DeliveryPreference,
                Status = MarketplaceOrderStatus.PendingPayment,
                CreatedAt = DateTime.UtcNow
            };

            context.MarketplaceOrders.Add(order);
            listing.Status = MarketplaceListingStatus.Sold;
            listing.UpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);

            return new CreateMarketplaceOrderResponse(
                order.Id,
                OrderNumberFormatter.Generate(order.Id, order.CreatedAt),
                listing.Title,
                order.Amount,
                order.Status.ToString(),
                order.Status.ToArabic(),
                "Order created successfully");
        
    }
   }
}
