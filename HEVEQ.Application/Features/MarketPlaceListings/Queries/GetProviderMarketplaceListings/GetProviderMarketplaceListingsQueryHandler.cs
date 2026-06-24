using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetProviderMarketplaceListings
{
    public class GetProviderMarketplaceListingsQueryHandler : IRequestHandler<GetProviderMarketplaceListingsQuery, IEnumerable<MarketPlaceListingDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProviderMarketplaceListingsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<IEnumerable<MarketPlaceListingDTO>> Handle(GetProviderMarketplaceListingsQuery request, CancellationToken cancellationToken)
        {
            int itemsToSkip = (request.PageNumber - 1) * request.PageSize;

            var providerListings = await _context.MarketplaceListings
                .Include(x => x.Seller)
                .Include(l => l.Category)
                .Include(l => l.Photos)
                .Where(l => l.SellerId == request.SellerId)
                .OrderByDescending(l => l.CreatedAt)
                .Skip(itemsToSkip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<MarketPlaceListingDTO>>(providerListings);


        }
    }
}
