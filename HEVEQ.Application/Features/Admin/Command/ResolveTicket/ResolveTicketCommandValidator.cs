using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ResolveTicket
{
    public class ResolveTicketCommandValidator : AbstractValidator<ResolveTicketCommand>
    {
        public ResolveTicketCommandValidator()
        {
            RuleFor(x => x.AdminResolution)
                .NotEmpty().WithMessage("Admin resolution note is required.");
        }
    }
}
