using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetProviderMarketplaceListings
{
    public record GetProviderMarketplaceListingsQuery:IRequest<PagedResult<ProviderMarketPlaceListingDTO>>
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
