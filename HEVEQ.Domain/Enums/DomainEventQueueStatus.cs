namespace HEVEQ.Domain.Enums;

public enum DomainEventQueueStatus
{
    Pending = 0,
    Processing = 1,
    Processed = 2,
    FailedRetryable = 3,
    DeadLetter = 4
}
