namespace HEVEQ.Domain.Enums;

public enum TicketStatus
{
    Open = 0,
    InProgress = 1,
    PendingCustomerReply = 2,
    PendingProviderReply = 3,
    PendingFieldVerification = 4,
    Resolved = 5,
    Closed = 6,
    Reopened = 7
}
