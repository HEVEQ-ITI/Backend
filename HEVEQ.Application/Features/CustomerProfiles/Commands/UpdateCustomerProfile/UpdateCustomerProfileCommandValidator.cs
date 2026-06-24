using FluentValidation;

namespace HEVEQ.Application.Features.CustomerProfiles.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandValidator
    : AbstractValidator<UpdateCustomerProfileCommand>
{
    public UpdateCustomerProfileCommandValidator()
    {
        // ───── User fields ─────

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("First name cannot be empty or whitespace.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Last name cannot be empty or whitespace.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(256);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{8,15}$")
            .WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        // ───── Address (optional object) ─────
        When(x => x.DefaultAddress != null, () =>
        {
            RuleFor(x => x.DefaultAddress!.Governorate)
                .NotEmpty().WithMessage("Governorate is required.")
                .MaximumLength(100)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Governorate cannot be empty or whitespace.");

            RuleFor(x => x.DefaultAddress!.District)
                .NotEmpty().WithMessage("District is required.")
                .MaximumLength(100)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("District cannot be empty or whitespace.");

            RuleFor(x => x.DefaultAddress!.Street)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.DefaultAddress!.Street));

            RuleFor(x => x.DefaultAddress!.Label)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.DefaultAddress!.Label));
        });
    }
}