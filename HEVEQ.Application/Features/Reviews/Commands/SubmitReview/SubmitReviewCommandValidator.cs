using FluentValidation;

namespace HEVEQ.Application.Features.Reviews.Commands.SubmitReview;

public class SubmitReviewCommandValidator : AbstractValidator<SubmitReviewCommand>
{
    public SubmitReviewCommandValidator()
    {
        // Business Rule: must reference exactly one transaction — not both, not neither
        RuleFor(x => x)
            .Must(x => x.BookingId.HasValue != x.MarketplaceOrderId.HasValue)
            .WithMessage("Review must reference either a booking or a marketplace order — not both and not neither.");

        // Business Rule: rating between 1 and 5
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5.");

        // Comment is optional but limited in length
        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithMessage("Comment cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}