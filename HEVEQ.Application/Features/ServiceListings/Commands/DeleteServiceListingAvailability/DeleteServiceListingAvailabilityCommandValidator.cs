using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListingAvailability;

public class DeleteServiceListingAvailabilityCommandValidator : AbstractValidator<DeleteServiceListingAvailabilityCommand>
{
    public DeleteServiceListingAvailabilityCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required.");

        RuleFor(x => x.AvailabilityId)
            .NotEmpty()
            .WithMessage("Availability ID is required.");
    }
}