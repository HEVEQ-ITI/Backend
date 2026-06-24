using HEVEQ.Application.Features.MarketPlace.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetProviderMarketplaceListings
{
    public record GetProviderMarketplaceListingsQuery:IRequest<IEnumerable<MarketPlaceListingDTO>>
    {
        public Guid SellerId { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
