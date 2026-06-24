using HEVEQ.Application.Features.MarketPlace.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListings
{
    public record GetMarketPlaceListingsQuery : IRequest<IEnumerable<MarketPlaceListingDTO>>
    {
        public int? CategoryId { get; init; }
        public string? Governorate { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? Search { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

}
