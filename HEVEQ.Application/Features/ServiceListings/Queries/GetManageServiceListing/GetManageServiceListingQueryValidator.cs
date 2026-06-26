using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetManageServiceListing;

public class GetManageServiceListingQueryValidator : AbstractValidator<GetManageServiceListingQuery>
{
    public GetManageServiceListingQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Service listing ID is required.");
    }
}