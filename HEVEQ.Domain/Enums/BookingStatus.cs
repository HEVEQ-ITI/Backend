namespace HEVEQ.Domain.Enums;

public enum BookingStatus
{
    Draft = 0,
    PendingProviderResponse = 1,
    ConfirmedPendingPayment = 2,
    Active = 3,
    InProgress = 4,
    PendingCustomerConfirmation = 5,
    Completed = 6,
    Disputed = 7,
    Rejected = 8,
    ProviderUnresponsive = 9,
    Reassigned = 10,
    CancelledRefunded = 11,
    ResolvedReleased = 12,
    ResolvedRefunded = 13,
    PendingFieldVerification = 14,
    FieldVerificationComplete = 15
}
