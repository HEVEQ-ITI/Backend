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
                var value = _httpContextAccessor.HttpContext?.User
                .FindFirstValue("uid");   

                return Guid.TryParse(value, out var id) ? id : null;
            }
        }

        public string? Role =>
       _httpContextAccessor.HttpContext?.User
           .FindFirstValue(ClaimTypes.Role);
    }
}
