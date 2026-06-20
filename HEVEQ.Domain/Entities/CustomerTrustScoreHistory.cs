namespace HEVEQ.Domain.Entities;

public class CustomerTrustScoreHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CustomerProfileId { get; set; }

    public decimal TrustScore { get; set; }

    public string? TriggerEvent { get; set; }

    public DateTime RecordedAt { get; set; }
    public CustomerProfile CustomerProfile { get; set; } = null!;

}
