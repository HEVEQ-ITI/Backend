using FluentValidation;

namespace HEVEQ.Application.Features.Tickets.Commands.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        // Business Rule: subject required
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(200).WithMessage("Subject cannot exceed 200 characters.");

        // Business Rule: message required
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters.");

        // Business Rule: cannot link booking AND marketplace order simultaneously
        RuleFor(x => x)
            .Must(x => !(x.BookingId.HasValue && x.MarketplaceOrderId.HasValue))
            .WithMessage("A ticket cannot be linked to both a booking and a marketplace order at the same time.");
    }
}