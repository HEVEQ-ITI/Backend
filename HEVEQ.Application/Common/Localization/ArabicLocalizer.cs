using HEVEQ.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Common.Localization
{
    public static class ArabicLocalizer
    {
        public static string ToArabic(this MarketplaceListingStatus status) => status switch
        {
            MarketplaceListingStatus.Draft => "مسودة",
            MarketplaceListingStatus.PendingReview => "قيد المراجعة",
            MarketplaceListingStatus.Active => "نشط",
            MarketplaceListingStatus.Rejected => "مرفوض",
            MarketplaceListingStatus.Sold => "تم البيع",
            MarketplaceListingStatus.Suspended => "مُعلَّق",
            MarketplaceListingStatus.Inactive => "غير نشط",
            _ => status.ToString()
        };

        public static string ToArabic(this ProductCondition condition) => condition switch
        {
            ProductCondition.New => "جديد",
            ProductCondition.Excellent => "ممتاز",
            ProductCondition.Good => "جيد",
            ProductCondition.Fair => "مقبول",
            ProductCondition.Used => "مستعمل",
            _ => condition.ToString()
        };

        public static string ToArabic(this MarketplaceOrderStatus status) => status switch
        {
            MarketplaceOrderStatus.PendingPayment => "في انتظار الدفع",
            MarketplaceOrderStatus.PaymentCaptured => "تم تحصيل المبلغ",
            MarketplaceOrderStatus.SellerConfirmed => "تم التأكيد من البائع",
            MarketplaceOrderStatus.Dispatched => "تم الشحن",
            MarketplaceOrderStatus.Delivered => "تم التوصيل",
            MarketplaceOrderStatus.Completed => "مكتمل",
            MarketplaceOrderStatus.Disputed => "متنازع عليه",
            MarketplaceOrderStatus.CancelledPreDispatch => "ملغي قبل الشحن",
            MarketplaceOrderStatus.CancelledPostDispatch => "ملغي بعد الشحن",
            MarketplaceOrderStatus.Refunded => "تم استرجاع المبلغ",
            _ => status.ToString()
        };

        public static string ToArabic(this DocumentVerificationStatus status) => status switch
        {
            DocumentVerificationStatus.Pending => "قيد المراجعة",
            DocumentVerificationStatus.Approved => "تم القبول",
            DocumentVerificationStatus.Rejected => "مرفوض",
            DocumentVerificationStatus.Expired => "منتهي الصلاحية",
            _ => status.ToString()
        };

        public static string ToArabic(this EscrowStatus status) => status switch
        {
            EscrowStatus.PendingCapture => "في انتظار التحصيل",
            EscrowStatus.Captured => "تم التحصيل",
            EscrowStatus.Held => "محتجز",
            EscrowStatus.Released => "تم الإفراج",
            EscrowStatus.Refunded => "تم استرجاع المبلغ",
            EscrowStatus.Frozen => "مجمّد",
            EscrowStatus.PartialSettled => "تمت التسوية جزئياً",
            _ => status.ToString()
        };

    }
}
