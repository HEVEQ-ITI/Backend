using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.UpdateAddress
{
    public class UpdateMyAddressCommandValidator : AbstractValidator<UpdateMyAddressCommand>
    {
        public UpdateMyAddressCommandValidator()
        {
            RuleFor(x => x.Label)
                .MaximumLength(100).WithMessage("Label cannot exceed 100 characters.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .When(x => x.Latitude.HasValue)
                .WithMessage("Latitude must be a valid value between -90 and 90.");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .When(x => x.Longitude.HasValue)
                .WithMessage("Longitude must be a valid value between -180 and 180.");
        }
    }
}
