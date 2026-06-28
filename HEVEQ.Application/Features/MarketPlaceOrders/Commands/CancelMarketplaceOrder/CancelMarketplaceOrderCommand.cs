using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder
{
    public record CancelMarketplaceOrderCommand(Guid OrderId,CancelMarketplaceOrderRequest Request) : IRequest<MarketplaceOrderDto>;
    public record CancelMarketplaceOrderRequest(string? Reason);

}
