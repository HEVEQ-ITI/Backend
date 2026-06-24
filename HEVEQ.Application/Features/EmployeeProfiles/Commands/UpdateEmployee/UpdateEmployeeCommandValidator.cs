using FluentValidation;

namespace HEVEQ.Application.Features.EmployeeProfiles.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator
    : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeProfileId)
            .NotEmpty().WithMessage("Employee profile id is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9]{8,15}$").WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.AssignedGovernorate)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.AssignedGovernorate));
    }
}