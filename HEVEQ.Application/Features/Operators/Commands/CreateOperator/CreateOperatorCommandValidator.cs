using FluentValidation;

namespace HEVEQ.Application.Features.Operators.Commands.CreateOperator;

public class CreateOperatorCommandValidator
    : AbstractValidator<CreateOperatorCommand>
{
    public CreateOperatorCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200);

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative.")
            .LessThanOrEqualTo(60).WithMessage("Years of experience seems invalid.")
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