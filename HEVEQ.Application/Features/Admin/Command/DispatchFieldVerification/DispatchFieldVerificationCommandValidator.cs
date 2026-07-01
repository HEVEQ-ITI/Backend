using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.DispatchFieldVerification
{
    public class DispatchFieldVerificationCommandValidator : AbstractValidator<DispatchFieldVerificationCommand>
    {
        public DispatchFieldVerificationCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty();
            RuleFor(x => x.EmployeeUserId).NotEmpty();
            RuleFor(x => x.DispatchInstructions)
                .NotEmpty().WithMessage("Dispatch instructions are required for the field employee.");
        }
    }
}
