using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateAccessToken(ApplicationUser user);

        RefreshToken GenerateRefreshToken();

    }
}
