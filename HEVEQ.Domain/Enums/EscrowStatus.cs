namespace HEVEQ.Domain.Enums;

public enum EscrowStatus
{
    PendingCapture = 0,
    Captured = 1,
    Held = 2,
    Released = 3,
    Refunded = 4,
    Frozen = 5,
    PartialSettled = 6
}
