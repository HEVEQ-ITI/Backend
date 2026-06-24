using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlace.Commands.AddMarketplaceListingPhoto
{
    public class AddMarketplaceListingPhotoCommandValidator:AbstractValidator<AddMarketplaceListingPhotoCommand>
    {
        public AddMarketplaceListingPhotoCommandValidator()
        {
            RuleFor(x => x.ListingId).NotEmpty();

            RuleFor(x => x.Request).ChildRules(request =>
            {
                request.RuleFor(r => r.PhotoUrl).NotEmpty().WithMessage("Photo URL is required.");
                request.RuleFor(r => r.DisplayOrder).GreaterThanOrEqualTo(0);
            });

        }


    }
}
