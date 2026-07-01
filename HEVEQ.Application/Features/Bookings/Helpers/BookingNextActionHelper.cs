using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Bookings.Helpers
{
    public static class BookingNextActionHelper
    {
        public static (string Label, string LabelAr, string? ActionKey) Build(BookingStatus status, string? role)
        {
            var isCustomer = string.Equals(role, "Customer", StringComparison.OrdinalIgnoreCase);
            var isProvider = string.Equals(role, "Provider", StringComparison.OrdinalIgnoreCase);
            var isAdmin = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);

            if (isAdmin)
                return ("Review booking status", "مراجعة حالة الحجز", null);

            return status switch
            {
                BookingStatus.PendingProviderResponse when isProvider =>
                    ("Accept or reject this booking request", "قبول أو رفض طلب الحجز", "accept_or_reject"),

                BookingStatus.PendingProviderResponse when isCustomer =>
                    ("Wait for provider response", "في انتظار رد المزود", null),

                BookingStatus.ConfirmedPendingPayment when isCustomer =>
                    ("Complete payment to activate booking", "أكمل الدفع لتفعيل الحجز", "pay"),

                BookingStatus.ConfirmedPendingPayment when isProvider =>
                    ("Wait for customer payment", "في انتظار دفع العميل", null),

                BookingStatus.Active when isProvider =>
                    ("Start the job", "ابدأ تنفيذ العمل", "start"),

                BookingStatus.Active when isCustomer =>
                    ("Wait for provider to start the job", "في انتظار بدء المزود للعمل", null),

                BookingStatus.InProgress when isProvider =>
                    ("Submit completion evidence", "إرسال إثبات إتمام العمل", "complete_by_provider"),

                BookingStatus.InProgress when isCustomer =>
                    ("Wait for provider completion", "في انتظار إنهاء العمل من المزود", null),

                BookingStatus.PendingCustomerConfirmation when isCustomer =>
                    ("Confirm completion or open a dispute", "أكد اكتمال العمل أو افتح نزاع", "confirm_or_dispute"),

                BookingStatus.PendingCustomerConfirmation when isProvider =>
                    ("Wait for customer confirmation", "في انتظار تأكيد العميل", null),

                BookingStatus.Completed =>
                    ("Booking completed", "الحجز مكتمل", null),

                BookingStatus.Disputed =>
                    ("Dispute is under review", "النزاع قيد المراجعة", null),

                BookingStatus.Rejected =>
                    ("Booking rejected", "تم رفض الحجز", null),

                BookingStatus.Cancelled =>
                    ("Booking cancelled", "تم إلغاء الحجز", null),

                BookingStatus.CancelledRefunded =>
                    ("Booking cancelled and refunded", "تم إلغاء الحجز ورد المبلغ", null),

                BookingStatus.ProviderUnresponsive =>
                    ("Provider did not respond in time", "المزود لم يرد في الوقت المحدد", null),

                _ => ("No action required", "لا يوجد إجراء مطلوب", null)
            };
        }
    }
}