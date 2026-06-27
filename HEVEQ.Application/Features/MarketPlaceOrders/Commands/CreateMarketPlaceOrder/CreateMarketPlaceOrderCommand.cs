using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder
{
    public record CreateMarketPlaceOrderCommand(CreateMarketPlaceOrderRequest Request) : IRequest<CreateMarketplaceOrderResponse>;
    
}
public record CreateMarketPlaceOrderRequest(
    Guid ListingId,
    string? DeliveryAddress,
    DeliveryPreference? DeliveryPreference
);