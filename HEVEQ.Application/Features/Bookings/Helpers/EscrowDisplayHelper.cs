using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Bookings.Helpers;
public static class EscrowDisplayHelper
{
    public static string GetStatusAr(this EscrowStatus status)
    {
        return status switch
        {
            EscrowStatus.PendingCapture => "في انتظار التحصيل",
            EscrowStatus.Captured => "تم التحصيل",
            EscrowStatus.Held => "محتجز",
            EscrowStatus.Released => "تم الإفراج",
            EscrowStatus.Refunded => "تم رد المبلغ",
            EscrowStatus.Frozen => "مجمّد",
            EscrowStatus.PartialSettled => "تسوية جزئية",
            _ => status.ToString()
        };
    }
}