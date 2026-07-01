using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command
{
    public class FieldVerificationDecisionCommandValidator : AbstractValidator<FieldVerificationDecisionCommand>
    {
        public FieldVerificationDecisionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.AdminDecision)
                .NotEmpty().WithMessage("Admin decision is required.");

            RuleFor(x => x.AdminDecisionNote)
                .NotEmpty().WithMessage("Admin decision note is required to justify the outcome.");
        }
    }
}
