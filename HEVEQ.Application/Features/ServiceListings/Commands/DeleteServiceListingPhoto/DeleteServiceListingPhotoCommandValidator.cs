using FluentValidation;

namespace HEVEQ.Application.Features.ServiceListings.Commands.DeleteServiceListingPhoto;

public class DeleteServiceListingPhotoCommandValidator : AbstractValidator<DeleteServiceListingPhotoCommand>
{
    public DeleteServiceListingPhotoCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty()
            .WithMessage("Listing Id is required.");

        RuleFor(x => x.PhotoId)
            .NotEmpty()
            .WithMessage("Photo Id is required.");
    }
}