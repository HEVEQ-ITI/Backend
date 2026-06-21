using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Auth.DTOs;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Auth.Commad.Regiser
{
    public class RegisterComandHandler (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,IJwtService jwtService, IApplicationDbContext context) : IRequestHandler<RegisterComand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(RegisterComand request, CancellationToken cancellationToken)
        {

            if (await userManager.FindByEmailAsync(request.Email) is not null) 
                return new AuthResponse
                {
                    Message = " Email is already exists"
                };
            if (await userManager.FindByNameAsync(request.UserName) is not null) 
                return new AuthResponse
                {
                    Message = " userName is already exists"
                };
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description} ,";
                return new AuthResponse
                {
                    Message = errors
                };
            }

            if (!await roleManager.RoleExistsAsync("Customer"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Customer"));

            await userManager.AddToRoleAsync(user, "Customer");

            var token = await jwtService.GenerateAccessToken(user);

            var refreshToken = jwtService.GenerateRefreshToken();

            //user.RefreshTokens.Add(refreshToken);
            //var isUpdated = await userManager.UpdateAsync(user);
           
            refreshToken.UserId = user.Id;
            context.RefreshTokens.Add(refreshToken);

            await context.SaveChangesAsync(cancellationToken);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = request.Email,
                Roles = new List<string> { "Customer" },
                UserName = request.UserName,
                IsAuthenticated = true,
                ExpiresAt = refreshToken.ExpiresAt,
                AccessToken = token,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
