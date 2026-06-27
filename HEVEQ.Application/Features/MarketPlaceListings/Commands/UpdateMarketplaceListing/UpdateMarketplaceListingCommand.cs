using HEVEQ.Application.Features.MarketPlace.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.UpdateMarketplaceListing
{
    public record UpdateMarketplaceListingCommand(Guid Id, UpdateMarketplaceListingRequest Request) : IRequest;

}
public record UpdateMarketplaceListingRequest(
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
    string? District,
    string? VideoUrl);
