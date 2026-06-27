using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetPendingMarketplaceListings
{
    public class GetPendingMarketplaceListingsQueryValidator : AbstractValidator<GetPendingMarketplaceListingsQuery>
    {
        public GetPendingMarketplaceListingsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
