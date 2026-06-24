using AutoMapper;
using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListingById
{
    public class GetMarketplaceListingByIdQueryHandler : IRequestHandler<GetMarketplaceListingByIdQuery, MarketplaceListingDetailsDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetMarketplaceListingByIdQueryHandler(IApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<MarketplaceListingDetailsDto> Handle(GetMarketplaceListingByIdQuery request, CancellationToken cancellationToken)
        {
            var listing = await context.MarketplaceListings
            .AsNoTracking()
            .Include(l => l.Seller)
            .Include(l => l.Category)
            .Include(l => l.Photos)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

            if (listing is null) return null;
            var isOwner = request.RequestingUserId.HasValue && listing.SellerId == request.RequestingUserId.Value;
            if (listing.Status != MarketplaceListingStatus.Active && !isOwner && !request.IsAdmin) return null;
            return mapper.Map<MarketplaceListingDetailsDto>(listing);
        }
    }
}
