using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Bookings.Helpers;

public static class BookingTimelineHelper
{
    public static IReadOnlyList<BookingTimelineItemDto> Build(Booking booking)
    {
        return new List<BookingTimelineItemDto>
        {
            new()
            {
                Key = "Created",
                Label = "Booking created",
                LabelAr = "تم إنشاء الحجز",
                Date = booking.CreatedAt,
                Done = true
            },
            new()
            {
                Key = "ProviderConfirmed",
                Label = "Provider confirmed",
                LabelAr = "تم تأكيد المزود",
                Date = booking.ConfirmedAt,
                Done = booking.ConfirmedAt.HasValue
                    || booking.Status is BookingStatus.ConfirmedPendingPayment
                        or BookingStatus.Active
                        or BookingStatus.InProgress
                        or BookingStatus.PendingCustomerConfirmation
                        or BookingStatus.Completed
                        or BookingStatus.Disputed
            },
            new()
            {
                Key = "Rejected",
                Label = "Booking rejected",
                LabelAr = "تم رفض الحجز",
                Date = booking.RejectedAt,
                Done = booking.Status == BookingStatus.Rejected
            },
            new()
            {
                Key = "PaymentCaptured",
                Label = "Payment captured",
                LabelAr = "تم الدفع",
                Date = booking.PaymentCapturedAt,
                Done = booking.PaymentCapturedAt.HasValue
                    || booking.Status is BookingStatus.Active
                        or BookingStatus.InProgress
                        or BookingStatus.PendingCustomerConfirmation
                        or BookingStatus.Completed
                        or BookingStatus.Disputed
            },
            new()
            {
                Key = "Started",
                Label = "Job started",
                LabelAr = "بدأ تنفيذ العمل",
                Date = booking.StartedAt,
                Done = booking.StartedAt.HasValue
                    || booking.Status is BookingStatus.InProgress
                        or BookingStatus.PendingCustomerConfirmation
                        or BookingStatus.Completed
                        or BookingStatus.Disputed
            },
            new()
            {
                Key = "ProviderCompleted",
                Label = "Provider marked completed",
                LabelAr = "المزود أنهى العمل",
                Date = booking.CompletedMarkedAt,
                Done = booking.CompletedMarkedAt.HasValue
                    || booking.Status is BookingStatus.PendingCustomerConfirmation
                        or BookingStatus.Completed
                        or BookingStatus.Disputed
            },
            new()
            {
                Key = "CustomerConfirmed",
                Label = "Customer confirmed completion",
                LabelAr = "العميل أكد اكتمال العمل",
                Date = booking.CompletionConfirmedAt,
                Done = booking.Status == BookingStatus.Completed
            },
            new()
            {
                Key = "Disputed",
                Label = "Dispute opened",
                LabelAr = "تم فتح نزاع",
                Date = booking.DisputeOpenedAt,
                Done = booking.Status == BookingStatus.Disputed
            }
        };
    }
}