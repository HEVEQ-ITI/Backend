using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketPlaceListing
{
    public class DeleteMarketPlaceListingCommandValidator:AbstractValidator<DeleteMarketPlaceListingCommand>
    {
        public DeleteMarketPlaceListingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Listing ID is required.");

            RuleFor(x => x.SellerId)
                .NotEmpty().WithMessage("Seller ID is required.");
        }
    }
}
