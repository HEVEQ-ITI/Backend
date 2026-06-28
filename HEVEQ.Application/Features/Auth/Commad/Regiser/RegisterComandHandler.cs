using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Features.Auth.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Auth.Commad.Regiser
{
    public class RegisterComandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IJwtService jwtService,
        IApplicationDbContext context) : IRequestHandler<RegisterComand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(RegisterComand request, CancellationToken cancellationToken)
        {
            if (await userManager.FindByEmailAsync(request.Email) is not null)
                return new AuthResponse { Message = "Email is already exists" };

            if (await userManager.FindByNameAsync(request.UserName) is not null)
                return new AuthResponse { Message = "UserName is already exists" };
            if (await userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber) is not null)
                return new AuthResponse { Message = "Phone number must be uniqe" };


            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description} ,";
                return new AuthResponse { Message = errors };
            }

            var allowedRoles = new[] { "Customer", "Provider"};
            var roleToAssign = allowedRoles.Contains(request.Role) ? request.Role : "Customer";

            if (!await roleManager.RoleExistsAsync(roleToAssign))
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleToAssign));

            await userManager.AddToRoleAsync(user, roleToAssign);

            // After: await userManager.AddToRoleAsync(user, roleToAssign);

            // Seed the matching profile row so profile endpoints work immediately
            if (roleToAssign == "Customer")
            {
                context.CustomerProfiles.Add(new HEVEQ.Domain.Entities.CustomerProfile
                {
                    UserId = user.Id,
                    TrustScore = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else if (roleToAssign == "Provider")
            {
                context.ProviderProfiles.Add(new HEVEQ.Domain.Entities.ProviderProfile
                {
                    UserId = user.Id,
                    CompanyName = $"{user.FirstName}'s Company",  // default, provider updates via PUT
                    ServiceRadiusKm = 10,
                    AverageRating = 0,
                    TotalReviewsCount = 0,
                    CompletedBookingsCount = 0,
                    ResponseRate = 0,
                    SearchRankingModifier = 1,
                    TrustScore = 0,
                    TrustLevel = HEVEQ.Domain.Enums.TrustLevel.Standard,
                    OnboardingTier = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await context.SaveChangesAsync(cancellationToken);

            var token = await jwtService.GenerateAccessToken(user);

            var refreshToken = jwtService.GenerateRefreshToken();

            refreshToken.UserId = user.Id;
            context.RefreshTokens.Add(refreshToken);

            await context.SaveChangesAsync(cancellationToken);

            return new AuthResponse
            {
                DisplayName = user.FirstName + " " + user.LastName,
                UserId = user.Id,
                Email = request.Email,
                Roles = new List<string> { roleToAssign },
                UserName = request.UserName,
                IsAuthenticated = true,
                ExpiresAt = refreshToken.ExpiresAt,
                AccessToken = token,
                RefreshToken = refreshToken.Token,
                ProfileCompleted = false
            };
        }
    }
}