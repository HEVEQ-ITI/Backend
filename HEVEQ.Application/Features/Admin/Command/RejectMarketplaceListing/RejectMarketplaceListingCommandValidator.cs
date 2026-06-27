using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RejectMarketplaceListing
{
    public class RejectMarketplaceListingCommandValidator : AbstractValidator<RejectMarketplaceListingCommand>
    {
        public RejectMarketplaceListingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Marketplace Listing ID is required.");

            // سبب الرفض إجباري
            RuleFor(x => x.AdminRejectionNote)
                .NotEmpty().WithMessage("Admin rejection note is required.")
                .MaximumLength(1000).WithMessage("Rejection note cannot exceed 1000 characters.");
        }
    }
}
