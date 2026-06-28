using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UnlinkOperatorFromListing;

public class UnlinkOperatorFromListingCommandValidator : AbstractValidator<UnlinkOperatorFromListingCommand>
{
    public UnlinkOperatorFromListingCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required.");

        RuleFor(x => x.OperatorId)
            .NotEmpty()
            .WithMessage("Operator ID is required.");
    }
}