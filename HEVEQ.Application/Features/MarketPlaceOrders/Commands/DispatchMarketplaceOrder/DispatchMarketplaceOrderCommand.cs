using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder
{
    public record DispatchMarketplaceOrderCommand(Guid OrderId, DispatchMarketplaceOrderRequest Request) : IRequest<OrderActionResponse>;
   public record DispatchMarketplaceOrderRequest(string? TrackingNumber);
}
