using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Queries.GetPublicServiceListings;

public class GetPublicServiceListingsQueryValidator : AbstractValidator<GetPublicServiceListingsQuery>
{
    public GetPublicServiceListingsQueryValidator()
    {
       
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
            .LessThanOrEqualTo(100).WithMessage("You cannot request more than 100 items per page.");

        RuleFor(x => x.MinRate)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum rate cannot be negative.")
            .When(x => x.MinRate.HasValue);

        RuleFor(x => x.MaxRate)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum rate cannot be negative.")
            .When(x => x.MaxRate.HasValue);

        RuleFor(x => x)
            .Must(x => x.MaxRate >= x.MinRate)
            .WithMessage("Maximum rate must be greater than or equal to minimum rate.")
            .When(x => x.MinRate.HasValue && x.MaxRate.HasValue);
    }
}