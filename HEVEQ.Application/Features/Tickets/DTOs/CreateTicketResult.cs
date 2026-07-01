namespace HEVEQ.Application.Features.Tickets.DTOs;

public class CreateTicketResult
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusAr { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}