using HEVEQ.Application.Common.Enums;

namespace HEVEQ.Application.Common.Helpers;
public static class ReferenceNumberGenerator
{
    public static string Generate(ReferenceNumberType type, Guid id)
    {
        var prefix = GetPrefix(type);
        return $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{id.ToString("N")[..6].ToUpper()}";
    }

    private static string GetPrefix(ReferenceNumberType type)
    {
        return type switch
        {
            ReferenceNumberType.Booking => "BK",
            ReferenceNumberType.Payment => "PY",
            ReferenceNumberType.MarketplaceOrder => "OR",
            ReferenceNumberType.Ticket => "TK",
            ReferenceNumberType.Document => "DC",
            ReferenceNumberType.ServiceListing => "SL",
            ReferenceNumberType.MarketplaceListing => "ML",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported reference number type.")
        };
    }
}