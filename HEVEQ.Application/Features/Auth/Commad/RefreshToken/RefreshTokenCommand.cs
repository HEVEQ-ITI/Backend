using HEVEQ.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.RefreshToken
{
    public record RefreshTokenCommand(string refreshToken) : IRequest<AuthResponse>;
    
}
