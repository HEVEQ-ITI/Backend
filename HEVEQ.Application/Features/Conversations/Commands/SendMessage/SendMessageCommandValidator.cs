using System.Text.RegularExpressions;
using FluentValidation;

namespace HEVEQ.Application.Features.Conversations.Commands.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    // Business Rule: basic contact-info blocking
    // Blocks: phone numbers (8-15 digits, optional +) and email addresses
    // TODO (AI phase): replace with ML-based detection for more sophisticated patterns
    private static readonly Regex ContactInfoPattern = new(
        @"(\+?[0-9]{8,15})|([a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,})",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public SendMessageCommandValidator()
    {
        RuleFor(x => x.Body)
            .NotEmpty()
            .WithMessage("Message body is required.")
            .MaximumLength(2000)
            .WithMessage("Message cannot exceed 2000 characters.")
            .Must(body => !ContactInfoPattern.IsMatch(body))
            .WithMessage(
                "Messages cannot contain phone numbers or email addresses. " +
                "Please use the platform for all communication.");
    }
}