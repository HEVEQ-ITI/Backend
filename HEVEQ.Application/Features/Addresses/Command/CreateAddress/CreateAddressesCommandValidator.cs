using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.AddAddress
{
    public class CreateAddressesCommandValidator: AbstractValidator<CreateAddressesCommand>
    {
        public CreateAddressesCommandValidator()
        {
            RuleFor(x => x.Governorate).NotNull().WithMessage("Governrate is required");
            RuleFor(x => x.Street).NotNull().WithMessage("Street is required");
            RuleFor(x => x.Latitude)
                 .InclusiveBetween(-90, 90)
                 .WithMessage("Latitude must be between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude must be between -180 and 180.");
            RuleFor(x => x.Label).NotNull().WithMessage("Label is required");
            RuleFor(x => x.District).NotNull().WithMessage("District is required");
        }
    }
}
