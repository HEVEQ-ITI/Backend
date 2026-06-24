namespace HEVEQ.Application.Features.CustomerProfiles.DTOs;

public class CustomerTrustHistoryDto
{
    public Guid Id { get; set; }
    public decimal TrustScore { get; set; }
    public string? TriggerEvent { get; set; }
    public DateTime RecordedAt { get; set; }
}