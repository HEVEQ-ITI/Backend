using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Body { get; set; }

    public string? ReferenceId { get; set; }

    public string? ReferenceType { get; set; }

    public bool IsRead { get; set; }

    public NotificationChannel Channel { get; set; }

    public DateTime SentAt { get; set; }

    public DateTime? ReadAt { get; set; }
    public ApplicationUser User { get; set; } = null!;

}
