using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Logout
{
    public class LogoutCommandValidator :AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(c => c.RefreshToken).NotEmpty().WithMessage("refreshToken is requieres");
        }
    }
}
