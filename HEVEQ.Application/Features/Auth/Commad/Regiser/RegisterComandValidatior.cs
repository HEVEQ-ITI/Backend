using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Regiser
{
    public class RegisterComandValidatior : AbstractValidator<RegisterComand>
    {
        public RegisterComandValidatior()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format"); 

            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is requiered");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is requiered");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is requiered");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is requiered");
        }
    }
}
