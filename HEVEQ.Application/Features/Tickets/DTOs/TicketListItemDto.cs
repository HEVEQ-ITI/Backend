namespace HEVEQ.Application.Features.Tickets.DTOs;

public class TicketListItemDto
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusAr { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Wraps the list + total for GET /api/tickets/my
public class MyTicketsResult
{
    public List<TicketListItemDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
}