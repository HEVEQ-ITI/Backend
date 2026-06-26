using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Auth.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.RefreshToken
{
    public class RefreshTokenCommandHandler
        (IApplicationDbContext context, IJwtService jwtService,UserManager<ApplicationUser> userManager) 
        : IRequestHandler<RefreshTokenCommand,AuthResponse>
    {
        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var auth = new AuthResponse();
            var user = await userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.RefreshTokens.Any(r => r.Token == request.refreshToken));

            if(user == null)
            {
                auth.Message = "Invalid Token";
                return auth;
            }

            var refreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == request.refreshToken);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                auth.Message = "Token is invalid, expired, or revoked";
                return auth;
            }
            refreshToken.IsRevoked = true;

            var newRefreshToken = jwtService.GenerateRefreshToken();
            newRefreshToken.UserId = user.Id;
            context.RefreshTokens.Add(newRefreshToken);

            await context.SaveChangesAsync(cancellationToken);

            auth.IsAuthenticated = true;
            auth.Email = user.Email;
            auth.UserId = user.Id;
            auth.UserName = user.UserName;
            var Roles = await userManager.GetRolesAsync(user);
            auth.Roles = Roles.ToList();
            auth.AccessToken = await jwtService.GenerateAccessToken(user);
            auth.RefreshToken = newRefreshToken.Token;
            auth.ExpiresAt = newRefreshToken.ExpiresAt;

            return auth;
        }
    }
}
