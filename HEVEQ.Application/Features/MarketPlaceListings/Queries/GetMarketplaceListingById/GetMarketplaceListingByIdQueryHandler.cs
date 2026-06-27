using AutoMapper;
using HEVEQ.Application.Common.Exceptions;
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
    public class GetMarketplaceListingByIdQueryHandler(IApplicationDbContext context, IMapper mapper,ICurrentUserService currentUser) : IRequestHandler<GetMarketplaceListingByIdQuery, MarketplaceListingDetailsDto>
    {
        public async Task<MarketplaceListingDetailsDto> Handle(GetMarketplaceListingByIdQuery request, CancellationToken cancellationToken)
        {
            var listing = await context.MarketplaceListings
            .AsNoTracking()
            .Include(l => l.Seller)
            .Include(l => l.Category)
            .Include(l => l.Photos)
            .FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(MarketplaceListing),request.Id);

            var isOwner = currentUser.IsAuthenticated && currentUser.UserId == listing.SellerId;
            var isAdmin = currentUser.IsAuthenticated && currentUser.Role == "Admin";

            if (listing.Status != MarketplaceListingStatus.Active && !isOwner && !isAdmin)
                throw new NotFoundException(nameof(MarketplaceListing), request.Id);

            return mapper.Map<MarketplaceListingDetailsDto>(listing);
        }
    }
}
