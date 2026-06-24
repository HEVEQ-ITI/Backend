using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketPlaceListing
{
    public record DeleteMarketPlaceListingCommand(Guid Id, Guid SellerId) : IRequest;
    
}
