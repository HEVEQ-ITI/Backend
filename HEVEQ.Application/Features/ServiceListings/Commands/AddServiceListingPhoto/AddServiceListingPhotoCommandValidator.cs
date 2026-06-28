using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.AddServiceListingPhoto;

public class AddServiceListingPhotoCommandValidator : AbstractValidator<AddServiceListingPhotoCommand>
{
    public AddServiceListingPhotoCommandValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.PhotoUrl).NotEmpty().WithMessage("PhotoUrl is required.");
        RuleFor(x => x.DisplayOrder).GreaterThanOrEqualTo(0).WithMessage("DisplayOrder must be zero or greater.");
    }
}