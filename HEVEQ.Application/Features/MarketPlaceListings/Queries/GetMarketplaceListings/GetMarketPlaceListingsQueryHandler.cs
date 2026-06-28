using AutoMapper;
using AutoMapper.QueryableExtensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListings
{
    public class GetMarketPlaceListingsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMarketPlaceListingsQuery, PagedResult<MarketPlaceListingDTO>>
    {
        public async Task<PagedResult<MarketPlaceListingDTO>> Handle(GetMarketPlaceListingsQuery request, CancellationToken cancellationToken)
        {
            var query = context.MarketplaceListings
                .AsNoTracking()
                .Where(l => l.Status == MarketplaceListingStatus.Active);

            if (request.CategoryId.HasValue)
                query = query.Where(l => l.CategoryId == request.CategoryId.Value);

            if (request.Condition.HasValue)
                query = query.Where(l => l.Condition == request.Condition.Value);

            if (!string.IsNullOrWhiteSpace(request.Governorate))
                query = query.Where(l => l.Governorate == request.Governorate);

            if (request.MinPrice.HasValue)
                query = query.Where(l => l.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(l => l.Price <= request.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(l => l.Title.Contains(request.Search));

            query = query.OrderByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            
            var entities = await query
                .Include(l => l.Seller)
                .Include(l => l.Category)
                .Include(l => l.Photos)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<MarketPlaceListingDTO>
            {
                Items = mapper.Map<List<MarketPlaceListingDTO>>(entities),
                TotalCount = totalCount
            };
        }
    }
}
