namespace HEVEQ.Application.Features.Tickets.DTOs;

// Single message in a ticket — internal admin notes are never included
public class TicketMessageDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

// Full ticket details for GET /api/tickets/{id}
public class TicketDetailsDto
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusAr { get; set; } = string.Empty;
    public List<TicketMessageDto> Messages { get; set; } = new();
}