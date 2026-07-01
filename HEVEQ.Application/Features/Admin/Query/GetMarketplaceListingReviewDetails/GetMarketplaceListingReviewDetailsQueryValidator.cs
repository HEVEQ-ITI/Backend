using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetMarketplaceListingReviewDetails
{
    public class GetMarketplaceListingReviewDetailsQueryValidator : AbstractValidator<GetMarketplaceListingReviewDetailsQuery>
    {
        public GetMarketplaceListingReviewDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Marketplace listing ID is required.");
        }
    }
}
