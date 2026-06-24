using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        bool IsAuthenticated { get; }
    }
}
