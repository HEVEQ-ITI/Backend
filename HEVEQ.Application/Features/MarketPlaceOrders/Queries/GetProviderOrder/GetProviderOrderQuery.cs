using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetProviderOrder
{
    public record GetProviderOrderQuery : IRequest<List<SaleOrderDto>>;
    
}
