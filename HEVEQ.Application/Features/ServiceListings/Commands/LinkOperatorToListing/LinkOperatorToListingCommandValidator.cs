using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.LinkOperatorToListing;

public class LinkOperatorToListingCommandValidator : AbstractValidator<LinkOperatorToListingCommand>
{
    public LinkOperatorToListingCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required.");

        RuleFor(x => x.OperatorId)
            .NotEmpty()
            .WithMessage("Operator ID is required.");
    }
}