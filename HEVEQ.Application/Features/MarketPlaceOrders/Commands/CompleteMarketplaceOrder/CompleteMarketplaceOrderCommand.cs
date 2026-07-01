using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder
{
    public record CompleteMarketplaceOrderCommand(Guid OrderId) : IRequest<OrderActionResponse>;
    
}
