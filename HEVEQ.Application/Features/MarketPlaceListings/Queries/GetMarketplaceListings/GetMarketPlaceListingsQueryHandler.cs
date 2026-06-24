using AutoMapper;
using AutoMapper.QueryableExtensions;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListings
{
    public class GetMarketPlaceListingsQueryHandler : IRequestHandler<GetMarketPlaceListingsQuery, IEnumerable<MarketPlaceListingDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMarketPlaceListingsQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MarketPlaceListingDTO>> Handle(GetMarketPlaceListingsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MarketplaceListings
            .AsNoTracking()
            .Where(l => l.Status == MarketplaceListingStatus.Active);       
           
            if (request.CategoryId.HasValue)
                query = query.Where(l => l.CategoryId == request.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(request.Governorate))
                query = query.Where(l => l.Governorate == request.Governorate);

            if (request.MinPrice.HasValue)
                query = query.Where(l => l.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(l => l.Price <= request.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(l => l.Title.Contains(request.Search));

            query = query.OrderByDescending(x => x.CreatedAt);
            var pagedQuery = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);


            return await pagedQuery
            .ProjectTo<MarketPlaceListingDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        }

    }
}
