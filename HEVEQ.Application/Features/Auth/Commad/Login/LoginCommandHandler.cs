using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Auth.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Login
{
    public class LoginCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService, IApplicationDbContext context) : IRequestHandler<LoginCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            var user = await userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(
                    x => x.Email == request.email,
                    cancellationToken); 
            if (user is null || !await userManager.CheckPasswordAsync(user,request.password))
                return new AuthResponse { Message = "Invalid Email or Password" };

            var auth = new AuthResponse();
            var userRoles = await userManager.GetRolesAsync(user);
            auth.DisplayName = user.FirstName + " " + user.LastName;
            auth.IsAuthenticated = true;
            auth.Email = request.email;
            auth.AccessToken = await jwtService.GenerateAccessToken(user);
            auth.UserName = user.UserName;
            auth.UserId = user.Id;
            auth.Roles = userRoles.ToList() ;
            var activeRefreshToken = user.RefreshTokens
            .FirstOrDefault(x =>
                !x.IsRevoked &&
                x.ExpiresAt > DateTime.UtcNow);

            if (activeRefreshToken is not null)
            {
                auth.RefreshToken = activeRefreshToken.Token;
                auth.ExpiresAt = activeRefreshToken.ExpiresAt;
            }
            else
            {
                var newRefreshToken = jwtService.GenerateRefreshToken();
                auth.RefreshToken = newRefreshToken.Token;
                auth.ExpiresAt = newRefreshToken.ExpiresAt;
                newRefreshToken.UserId = user.Id;

                context.RefreshTokens.Add(newRefreshToken);
                Console.WriteLine($"User Id = {user.Id}");
                Console.WriteLine($"Refresh UserId = {newRefreshToken.UserId}");

                await context.SaveChangesAsync(cancellationToken);
            }
            if(userRoles.FirstOrDefault() == "Customer")
            {
                var address = context.Addresses.Any(x => x.UserId == user.Id);
                var docs = context.Documents.Any(x=> x.UserId == user.Id);
                
                if(address || docs)
                    auth.ProfileCompleted = true;
                
            }
            else if(userRoles.FirstOrDefault() == "Provider")
            {
                var provider = await context.ProviderProfiles
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
                if (provider is not null)
                {
                    auth.ProfileCompleted =
                        !string.IsNullOrEmpty(provider.CompanyName) &&
                        provider.BaseLatitude.HasValue &&
                        provider.BaseLongitude.HasValue &&
                        provider.ServiceRadiusKm > 0 &&
                        provider.ResponseRate > 0;
                }
            }

            return auth;
        }
    }
}
