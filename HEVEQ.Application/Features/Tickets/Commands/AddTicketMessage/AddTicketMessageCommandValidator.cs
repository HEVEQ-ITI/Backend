using FluentValidation;

namespace HEVEQ.Application.Features.Tickets.Commands.AddTicketMessage;

public class AddTicketMessageCommandValidator
    : AbstractValidator<AddTicketMessageCommand>
{
    public AddTicketMessageCommandValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("Message body is required.")
            .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");
    }
}