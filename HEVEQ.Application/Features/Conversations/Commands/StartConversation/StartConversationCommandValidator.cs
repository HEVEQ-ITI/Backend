using FluentValidation;

namespace HEVEQ.Application.Features.Conversations.Commands.StartConversation;

public class StartConversationCommandValidator
    : AbstractValidator<StartConversationCommand>
{
    private static readonly string[] AllowedContextTypes =
        { "Booking", "ServiceListing", "MarketplaceListing" };

    public StartConversationCommandValidator()
    {
        RuleFor(x => x.ContextType)
            .NotEmpty()
            .WithMessage("ContextType is required.")
            .Must(t => AllowedContextTypes.Contains(t))
            .WithMessage("ContextType must be one of: Booking, ServiceListing, MarketplaceListing.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty()
            .WithMessage("ReferenceId is required.");
    }
}