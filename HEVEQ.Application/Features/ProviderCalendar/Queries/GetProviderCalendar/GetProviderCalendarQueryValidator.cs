using FluentValidation;

namespace HEVEQ.Application.Features.ProviderCalendar.Queries.GetProviderCalendar;

public class GetProviderCalendarQueryValidator : AbstractValidator<GetProviderCalendarQuery>
{
    public GetProviderCalendarQueryValidator()
    {
        // PDF business rule: "نطاق التاريخ مطلوب للفلترة" — date range is required.
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();

        RuleFor(x => x).Must(x => x.To >= x.From)
            .WithMessage("'To' date must be on or after 'From' date.");

        // Guards against an accidental unbounded scan (e.g. ?from=2000-01-01&to=2099-12-31).
        RuleFor(x => x).Must(x => (x.To.ToDateTime(TimeOnly.MinValue) - x.From.ToDateTime(TimeOnly.MinValue)).TotalDays <= 366)
            .WithMessage("Date range cannot exceed 366 days.");
    }
}