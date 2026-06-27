using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UpdateServiceListingAvailability;

public class UpdateServiceListingAvailabilityCommandValidator : AbstractValidator<UpdateServiceListingAvailabilityCommand>
{
    public UpdateServiceListingAvailabilityCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing ID is required.");

        RuleFor(x => x.AvailabilityId)
            .NotEmpty()
            .WithMessage("Availability ID is required.");

        RuleFor(x => x.DayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("DayOfWeek must be between 0 (Sunday) and 6 (Saturday).");

        RuleFor(x => x.OpenTime)
            .LessThan(x => x.CloseTime)
            .WithMessage("OpenTime must be strictly before CloseTime.");
    }
}