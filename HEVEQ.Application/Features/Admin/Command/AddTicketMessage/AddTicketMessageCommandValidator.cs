using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.AddTicketMessage
{
    public class AddTicketMessageCommandValidator : AbstractValidator<AddTicketMessageCommand>
    {
        public AddTicketMessageCommandValidator()
        {
            RuleFor(x => x.TicketId).NotEmpty();

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Message body is required.")
                .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");
        }
    }
}
