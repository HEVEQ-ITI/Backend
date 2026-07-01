using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public record OrderActionResponse(
        Guid OrderId,
        string Status,
        string StatusAr,
        string Message);
}
