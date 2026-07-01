using FluentValidation;

namespace HEVEQ.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator()
    {
        // Enforce that IF a Type is provided, it must be a valid, defined value within the CategoryType Enum
        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue)
            .WithMessage("The provided category type is invalid. It must be either Service or Marketplace.");
    }
}