using FluentValidation;

namespace HEVEQ.Application.Features.Operators.Commands.UpdateOperator;

public class UpdateOperatorCommandValidator
    : AbstractValidator<UpdateOperatorCommand>
{
    public UpdateOperatorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Operator id is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200);

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(60)
            .When(x => x.YearsOfExperience.HasValue);

        RuleFor(x => x.LicenseNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.LicenseNumber));

        RuleFor(x => x.LicenseExpiryDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("License expiry date must be in the future.")
            .When(x => x.LicenseExpiryDate.HasValue);
    }
}