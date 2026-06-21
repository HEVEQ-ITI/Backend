using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Login
{
    public class LoginCommandValidator :AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.email).NotEmpty().WithMessage("Email is Required");
            RuleFor(x => x.email).EmailAddress().WithMessage("this is not a correct email format");
            RuleFor(x => x.password).NotEmpty().WithMessage("Password is Required");
        }
    }
}
