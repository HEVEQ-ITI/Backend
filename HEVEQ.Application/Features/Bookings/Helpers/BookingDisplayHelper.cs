using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Bookings.Helpers;

public static class BookingDisplayHelper
{
    public static string GetStatusAr(this BookingStatus status)
    {
        return status switch
        {
            BookingStatus.Draft => "مسودة",
            BookingStatus.PendingProviderResponse => "في انتظار رد المزود",
            BookingStatus.ConfirmedPendingPayment => "مؤكد في انتظار الدفع",
            BookingStatus.Active => "نشط",
            BookingStatus.InProgress => "قيد التنفيذ",
            BookingStatus.PendingCustomerConfirmation => "في انتظار تأكيد العميل",
            BookingStatus.Completed => "مكتمل",
            BookingStatus.Disputed => "محل نزاع",
            BookingStatus.Rejected => "مرفوض",
            BookingStatus.ProviderUnresponsive => "المزود لم يرد",
            BookingStatus.Reassigned => "تم إعادة التعيين",
            BookingStatus.CancelledRefunded => "ملغي وتم رد المبلغ",
            BookingStatus.ResolvedReleased => "تم الحل وصرف المبلغ",
            BookingStatus.ResolvedRefunded => "تم الحل ورد المبلغ",
            BookingStatus.PendingFieldVerification => "في انتظار التحقق الميداني",
            BookingStatus.FieldVerificationComplete => "تم التحقق الميداني",
            BookingStatus.Cancelled => "ملغي",
            _ => status.ToString()
        };
    }
}