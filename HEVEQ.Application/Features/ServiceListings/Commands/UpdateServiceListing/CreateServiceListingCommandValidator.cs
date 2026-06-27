using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.UpdateServiceListing;

public class CreateServiceListingCommandValidator : AbstractValidator<UpdateServiceListingCommand>
{
    public CreateServiceListingCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0).WithMessage("HourlyRate must be zero or greater.");
        RuleFor(x => x.MinimumBookingHours).GreaterThanOrEqualTo(1).WithMessage("MinimumBookingHours must be at least 1.");
    }
}