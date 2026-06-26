using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListing;

public class DeleteServiceListingCommandValidator : AbstractValidator<DeleteServiceListingCommand>
{
    public DeleteServiceListingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Listing Id is required.");
    }
}