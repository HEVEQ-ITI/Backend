using HEVEQ.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Login
{
    public record LoginCommand(string email, string password) : IRequest<AuthResponse>;
    
}
