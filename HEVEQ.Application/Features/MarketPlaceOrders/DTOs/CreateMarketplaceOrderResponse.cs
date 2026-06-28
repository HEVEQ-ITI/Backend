using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.DTOs
{
    public record CreateMarketplaceOrderResponse(
       Guid Id,
       string OrderNumber,
       string ListingTitle,
       decimal Amount,
       string Status,
       string StatusAr,
       string Message);
}
