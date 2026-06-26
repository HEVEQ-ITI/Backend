using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Common.Extensions;

public static class ServiceListingStatusExtensions
{

    public static string ToArabicText(this ServiceListingStatus status) => status switch
    {
        ServiceListingStatus.Draft => "مسودة",
        ServiceListingStatus.PendingReview => "قيد المراجعة",
        ServiceListingStatus.Active => "متاح",
        ServiceListingStatus.Rejected => "مرفوض",
        ServiceListingStatus.Suspended => "معلق",
        ServiceListingStatus.Inactive => "غير نشط",
        _ => status.ToString()
    };
}