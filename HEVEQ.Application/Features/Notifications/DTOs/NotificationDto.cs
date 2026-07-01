namespace HEVEQ.Application.Features.Notifications.DTOs;

public class NotificationItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; }
}

public class NotificationListDto
{
    public List<NotificationItemDto> Items { get; set; } = new();

    // Always the TOTAL unread count — independent of the isRead filter applied to Items
    public int UnreadCount { get; set; }

    // Total matching the current filter (used by Angular for pagination display)
    public int TotalCount { get; set; }
}

public record MarkNotificationReadResult(Guid Id, bool IsRead, string Message);