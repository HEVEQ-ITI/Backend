using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketPlaceOrder
{
    public record ConfirmMarketplaceOrderCommand(Guid OrderId, Guid SellerId) : IRequest<MarketplaceOrderDto>;
    
}
