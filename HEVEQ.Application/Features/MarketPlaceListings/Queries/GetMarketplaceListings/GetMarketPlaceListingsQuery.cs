using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListings
{
    public record GetMarketPlaceListingsQuery : IRequest<PagedResult<MarketPlaceListingDTO>>
    {
        public int? CategoryId { get; init; }
        public ProductCondition? Condition { get; init; }
        public string? Governorate { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public string? Search { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }

}
