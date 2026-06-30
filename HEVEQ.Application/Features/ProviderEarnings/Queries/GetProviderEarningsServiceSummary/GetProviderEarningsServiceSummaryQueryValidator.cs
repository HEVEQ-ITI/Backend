using FluentValidation;

namespace HEVEQ.Application.Features.ProviderEarnings.Queries.GetProviderEarningsServiceSummary;

public class GetProviderEarningsServiceSummaryQueryValidator : AbstractValidator<GetProviderEarningsServiceSummaryQuery>
{
    public GetProviderEarningsServiceSummaryQueryValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
        RuleFor(x => x).Must(x => x.To >= x.From)
            .WithMessage("'To' date must be on or after 'From' date.");
    }
}