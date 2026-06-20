namespace HEVEQ.Domain.Enums;

public enum MarketplaceOrderStatus
{
    PendingPayment = 0,
    PaymentCaptured = 1,
    SellerConfirmed = 2,
    Dispatched = 3,
    Delivered = 4,
    Completed = 5,
    Disputed = 6,
    CancelledPreDispatch = 7,
    CancelledPostDispatch = 8,
    Refunded = 9
}
