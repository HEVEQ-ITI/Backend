using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ApproveMarketplaceListing
{
    public class ApproveMarketplaceListingCommandValidator : AbstractValidator<ApproveMarketplaceListingCommand>
    {
        public ApproveMarketplaceListingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Marketplace Listing ID is required.");
        }
    }
}
