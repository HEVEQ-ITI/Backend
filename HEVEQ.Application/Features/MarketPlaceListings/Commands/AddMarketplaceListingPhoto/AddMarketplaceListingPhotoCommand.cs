using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.AddMarketplaceListingPhoto
{
    public record AddMarketplaceListingPhotoCommand (Guid ListingId,AddMarketplacePhotoRequest Request) : IRequest<Guid>;
    
}
public record AddMarketplacePhotoRequest(
    string PhotoUrl,
    int DisplayOrder);