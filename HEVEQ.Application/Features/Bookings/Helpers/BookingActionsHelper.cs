using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Bookings.Helpers;

public static class BookingActionsHelper
{
    public static bool CanCustomerCancel(BookingStatus status)
    {
        return status is BookingStatus.PendingProviderResponse or BookingStatus.ConfirmedPendingPayment or BookingStatus.Active;
    }

    public static bool CanCustomerConfirmCompletion(BookingStatus status)
    {
        return status == BookingStatus.PendingCustomerConfirmation;
    }

    public static bool CanCustomerDispute(BookingStatus status)
    {
        return status == BookingStatus.PendingCustomerConfirmation;
    }

    public static bool CanProviderAccept(BookingStatus status)
    {
        return status == BookingStatus.PendingProviderResponse;
    }

    public static bool CanProviderReject(BookingStatus status)
    {
        return status == BookingStatus.PendingProviderResponse;
    }

    public static bool CanProviderStart(BookingStatus status, Guid? assignedOperatorId)
    {
        return status == BookingStatus.Active && assignedOperatorId.HasValue;
    }

    public static bool CanProviderComplete(BookingStatus status)
    {
        return status == BookingStatus.InProgress;
    }

    public static bool CanProviderCancel(BookingStatus status)
    {
        return status is BookingStatus.ConfirmedPendingPayment or BookingStatus.Active;
    }

    public static bool CanCustomerPay(BookingStatus status)
    {
        return status == BookingStatus.ConfirmedPendingPayment;
    }
}