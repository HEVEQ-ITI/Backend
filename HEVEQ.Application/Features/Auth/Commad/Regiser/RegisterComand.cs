using HEVEQ.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Regiser
{
    public record RegisterComand(string FirstName, string LastName,string UserName, string Password, string Email): IRequest<AuthResponse>
    {
    }
}
