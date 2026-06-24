using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HEVEQ.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtHelper jwt;
        private readonly UserManager<ApplicationUser> userManager;

        public JwtService(
            IOptions<JwtHelper> jwt,
            UserManager<ApplicationUser> userManager)
        {
            this.jwt = jwt.Value;
            this.userManager = userManager;
        }


        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            // Get user roles
            var userRoles = await userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in userRoles)
            {
                roleClaims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role
                    )
                );
            }


            // Get custom user claims if exist
            var userClaims = await userManager.GetClaimsAsync(user);


            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                ),

                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserName!
                ),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email!
                ),

               new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);



            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.Key)
            );


            var signingCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


            var tokenObject = new JwtSecurityToken(
                claims: claims,
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                expires: DateTime.UtcNow.AddMinutes(jwt.DaurationInMinuts),
                signingCredentials: signingCredentials
            );


            return new JwtSecurityTokenHandler()
                .WriteToken(tokenObject);
        }



        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(64)
                ),

                CreatedAt = DateTime.UtcNow,

                ExpiresAt = DateTime.UtcNow.AddDays(15)
            };
        }
    }
}