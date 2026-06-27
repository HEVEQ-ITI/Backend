using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Addresses.Command.SetDefaultAddress
{
    public class SetDefaultAddressCommandValidator : AbstractValidator<SetDefaultAddressCommand>
    {
        public SetDefaultAddressCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Address ID is required.");
        }
    }
}
