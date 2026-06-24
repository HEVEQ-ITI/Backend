using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketplaceListingPhoto
{
    public record DeleteMarketplaceListingPhotoCommand(Guid ListingId, Guid PhotoId) : IRequest;
    
}
