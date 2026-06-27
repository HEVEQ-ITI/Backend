using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceOrderTracking
{
    public record GetMarketplaceOrderTrackingQuery(Guid OrderId) : IRequest<OrderTrackingDto>;
    
}
