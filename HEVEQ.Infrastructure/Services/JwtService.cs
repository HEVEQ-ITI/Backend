using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HEVEQ.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        public JwtHelper jwt;
        public UserManager<ApplicationUser> usermanger;
        public JwtService(IOptions<JwtHelper> jwt, UserManager<ApplicationUser> userManager)
        {
            this.jwt = jwt.Value;
            this.usermanger = userManager;
        }
        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userRoles = await usermanger.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach(var role in userRoles)
            {
                roleClaims.Add(new Claim("role", role));
            }
            var userClaims = await usermanger.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenObject = new JwtSecurityToken(
                claims: claims,
                issuer : jwt.Issuer,
                audience : jwt.Audience,
                expires: DateTime.UtcNow.AddMinutes(jwt.DaurationInMinuts),
                signingCredentials : signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }

        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(15)
            };
        }
    }
}
