using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Common
{
    public static class OrderNumberFormatter
    {
        public static string Generate(Guid orderId, DateTime createdAtUtc) =>
        $"ORD-{createdAtUtc:yyMM}-{orderId.ToString("N")[..6].ToUpperInvariant()}";
    }
}
