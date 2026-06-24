using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.DeleteMarketplaceListingPhoto
{
    public class DeleteMarketplaceListingPhotoCommandValidator:AbstractValidator<DeleteMarketplaceListingPhotoCommand>
    {
        public DeleteMarketplaceListingPhotoCommandValidator()
        {
            RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");

            RuleFor(x => x.PhotoId)
                .NotEmpty().WithMessage("Photo ID is required.");
        }

    }
}
