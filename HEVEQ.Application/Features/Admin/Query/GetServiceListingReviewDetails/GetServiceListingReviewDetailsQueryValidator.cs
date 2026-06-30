using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Query.GetServiceListingReviewDetails
{
    public class GetServiceListingReviewDetailsQueryValidator : AbstractValidator<GetServiceListingReviewDetailsQuery>
    {
        public GetServiceListingReviewDetailsQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Service listing ID is required.");
        }
    }
    
}
