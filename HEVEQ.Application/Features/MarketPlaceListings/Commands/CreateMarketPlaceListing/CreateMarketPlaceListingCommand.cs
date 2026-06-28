using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Application.Features.MarketPlaceListings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.CreateMarketPlaceListing
{
    public record CreateMarketPlaceListingCommand(CreateMarketplaceListingRequest Request) : IRequest<CreateMarketplaceListingResponse>;
    
}

public record CreateMarketplaceListingRequest(
    int CategoryId,
    string Title,
    ProductCondition Condition,
    int? YearOfManufacture,
    string Description,
    string? Specifications,
    decimal Price,
    bool IsNegotiable,
    MarketplaceTransactionMethod TransactionMethod,
    string Governorate,
    string? District);