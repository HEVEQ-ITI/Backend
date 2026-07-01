using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public record OpenDisputeResponse(
       Guid OrderId,
       string Status,
       string StatusAr,
       Guid TicketId,
       string EscrowStatus,
       string Message);
}
