using FluentValidation;

namespace HEVEQ.Application.Features.ProviderProfiles.Commands.UpdateProviderProfile;

public class UpdateProviderProfileCommandValidator
    : AbstractValidator<UpdateProviderProfileCommand>
{
    public UpdateProviderProfileCommandValidator()
    {
        // ── User fields ───────────────────────────────────────────────────
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z0-9._-]+$")
            .WithMessage("Username can only contain letters, numbers, dots, underscores and hyphens.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")
            .MaximumLength(256);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{8,15}$").WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        // ── Company fields ────────────────────────────────────────────────
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200);

        RuleFor(x => x.BusinessDescription)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrWhiteSpace(x.BusinessDescription));

        RuleFor(x => x.ServiceRadiusKm)
            .GreaterThan(0).WithMessage("Service radius must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Service radius cannot exceed 50 km.");
        // ── Location — both must be provided together or not at all ───────
        RuleFor(x => x.BaseLongitude)
            .NotNull().WithMessage("Longitude is required when Latitude is provided.")
            .When(x => x.BaseLatitude.HasValue);

        RuleFor(x => x.BaseLatitude)
            .NotNull().WithMessage("Latitude is required when Longitude is provided.")
            .When(x => x.BaseLongitude.HasValue);

        RuleFor(x => x.BaseLatitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
            .When(x => x.BaseLatitude.HasValue);

        RuleFor(x => x.BaseLongitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
            .When(x => x.BaseLongitude.HasValue);
    }
}