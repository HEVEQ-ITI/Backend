using HEVEQ.Application.Features.MarketPlace.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Queries.GetMarketplaceListingById
{
    public record GetMarketplaceListingByIdQuery(Guid Id) : IRequest<MarketplaceListingDetailsDto>;
  
}
