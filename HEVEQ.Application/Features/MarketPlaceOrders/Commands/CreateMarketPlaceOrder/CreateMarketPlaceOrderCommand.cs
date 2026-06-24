using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder
{
    public record CreateMarketPlaceOrderCommand(Guid BuyerId,CreateMarketPlaceOrderRequest Request) : IRequest<Guid>;
    
}
public record CreateMarketPlaceOrderRequest(
    Guid ListingId,
    string? DeliveryAddress,
    DeliveryPreference? DeliveryPreference
);