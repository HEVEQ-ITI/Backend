using HEVEQ.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Logout
{
    public class LogoutCommandHandler(IApplicationDbContext context) : IRequestHandler<LogoutCommand,bool>
    {
        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await context.RefreshTokens
           .FirstOrDefaultAsync(
               x => x.Token == request.RefreshToken,
               cancellationToken);

            if (refreshToken is null)
                return false;

            refreshToken.IsRevoked = true;

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
