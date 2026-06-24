using HEVEQ.Application.Common.Interfaces;
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
    public class CreateMarketPlaceOrderCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateMarketPlaceOrderCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMarketPlaceOrderCommand command, CancellationToken cancellationToken)
        {

            var request = command.Request;
            var listing = await context.MarketplaceListings
            .FirstOrDefaultAsync(l => l.Id == request.ListingId, cancellationToken);

            if (listing is null)
                throw new KeyNotFoundException($"Marketplace listing with ID {request.ListingId} was not found.");


            if (listing.Status != MarketplaceListingStatus.Active)
                throw new Exception("Orders can only be placed on approved, active listings.");

            if (listing.SellerId == command.BuyerId)
                throw new UnauthorizedAccessException("You cannot purchase your own listing");

            var order = new MarketplaceOrder
            {
                BuyerId = command.BuyerId,
                ListingId = listing.Id,
                Amount = listing.Price,
                DeliveryAddress = request.DeliveryAddress,
                DeliveryPreference = request.DeliveryPreference,
                Status = MarketplaceOrderStatus.PendingPayment,
                CreatedAt = DateTime.UtcNow
            };
            context.MarketplaceOrders.Add(order);
            listing.Status = MarketplaceListingStatus.Sold;
            listing.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
    }
}
