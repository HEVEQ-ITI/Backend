using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Common
{
   public static class TrackingNumberFormatter
    {
        public static string Generate(Guid orderId, DateTime dispatchedAtUtc) =>
        $"TRK-{dispatchedAtUtc:yyMM}-{orderId.ToString("N")[..6].ToUpperInvariant()}";
    }
}
