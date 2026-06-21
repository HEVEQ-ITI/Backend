using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Logout
{
    public record LogoutCommand(string RefreshToken) : IRequest<bool>;
    
}
