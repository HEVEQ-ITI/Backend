using HEVEQ.Application.Features.MarketPlaceOrders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Queries.GetMarketplaceEarningsSummary
{
    public record GetMarketplaceEarningsSummaryQuery: IRequest<MarketplaceEarningsSummaryDto>
    {
        public DateOnly? From { get; init; }
        public DateOnly? To { get; init; }
    }
}
