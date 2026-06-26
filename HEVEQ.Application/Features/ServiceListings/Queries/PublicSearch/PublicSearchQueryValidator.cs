using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Queries.PublicSearch;

public class PublicSearchQueryValidator : AbstractValidator<PublicSearchQuery>
{
    public PublicSearchQueryValidator()
    {
        //RuleFor(v => v.Query)
        //    .NotEmpty().WithMessage("Search query keyword is required.");

        //RuleFor(v => v.Context)
        //    .NotEmpty().WithMessage("Search context (services/marketplace) is required.");

        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.");

    }
}
