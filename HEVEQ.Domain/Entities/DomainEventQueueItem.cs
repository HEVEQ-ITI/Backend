using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class DomainEventQueueItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string EventType { get; set; } = string.Empty;

    public string EntityType { get; set; } = string.Empty;

    public Guid EntityId { get; set; }

    public DomainEventQueueStatus Status { get; set; }

    public int RetryCount { get; set; }

    public string? FailureReason { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}
