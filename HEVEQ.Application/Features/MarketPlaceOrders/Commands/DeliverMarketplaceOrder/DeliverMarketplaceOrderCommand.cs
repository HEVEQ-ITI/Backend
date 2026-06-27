using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder
{
    public record DeliverMarketplaceOrderCommand(Guid OrderId) : IRequest<MarketplaceOrderDto>;
    
}
