using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.Bookings.Helpers;
public static class TimeAdjustmentDisplayHelper
{
    public static string GetStatusAr(BookingTimeAdjustmentStatus status)
    {
        return status switch
        {
            BookingTimeAdjustmentStatus.Pending => "في انتظار موافقة العميل",
            BookingTimeAdjustmentStatus.Approved => "تمت الموافقة",
            BookingTimeAdjustmentStatus.Rejected => "مرفوض",
            BookingTimeAdjustmentStatus.Expired => "منتهي",
            _ => status.ToString()
        };
    }
}