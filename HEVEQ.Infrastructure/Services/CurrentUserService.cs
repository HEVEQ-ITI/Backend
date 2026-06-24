using HEVEQ.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HEVEQ.Infrastructure.Services
{
    public class CurrentUserService :ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                var userIdClaim =
                    user?.FindFirstValue("uid")
                    ?? user?.FindFirstValue(ClaimTypes.NameIdentifier);

                return Guid.TryParse(userIdClaim, out var parsedGuid) ? parsedGuid : null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    }
}
