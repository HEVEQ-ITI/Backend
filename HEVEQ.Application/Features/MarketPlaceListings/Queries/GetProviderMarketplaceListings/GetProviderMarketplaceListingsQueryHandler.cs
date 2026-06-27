using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Models;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetProviderMarketplaceListings
{
    public class GetProviderMarketplaceListingsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<GetProviderMarketplaceListingsQuery, PagedResult<ProviderMarketPlaceListingDTO>>
    {
        public async Task<PagedResult<ProviderMarketPlaceListingDTO>> Handle(GetProviderMarketplaceListingsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.UserId.HasValue)
                throw new ForbiddenAccessException("You must be logged in to view your listings.");

            var sellerId = currentUser.UserId.Value;

            var query = context.MarketplaceListings
                .Where(l => l.SellerId == sellerId)
                .OrderByDescending(l => l.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var providerListings = await query
                .Include(x => x.Seller)
                .Include(l => l.Category)
                .Include(l => l.Photos)
                .Skip((request.Page- 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ProviderMarketPlaceListingDTO>
            {
                Items = mapper.Map<List<ProviderMarketPlaceListingDTO>>(providerListings),
                TotalCount = totalCount
            };




        }
    }
}
