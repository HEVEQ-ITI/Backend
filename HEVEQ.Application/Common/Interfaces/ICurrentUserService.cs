using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Role { get; }
    }
}
