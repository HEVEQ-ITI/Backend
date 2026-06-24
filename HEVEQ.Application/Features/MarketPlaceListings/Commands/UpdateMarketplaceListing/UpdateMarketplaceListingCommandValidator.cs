using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.UpdateMarketplaceListing
{
    public class UpdateMarketplaceListingCommandValidator:AbstractValidator<UpdateMarketplaceListingCommand>
    {
        public UpdateMarketplaceListingCommandValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty();
            RuleFor(x => x.Request.CategoryId).GreaterThan(0);
            RuleFor(x => x.Request.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Request.Description).NotEmpty();
            RuleFor(x => x.Request.Specifications).MaximumLength(4000);
            RuleFor(x => x.Request.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Request.Governorate).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Request.District).MaximumLength(100);
            RuleFor(x => x.Request.VideoUrl).MaximumLength(500);
            RuleFor(x => x.Request.Condition).IsInEnum();
            RuleFor(x => x.Request.TransactionMethod).IsInEnum();
        }
    }
}
