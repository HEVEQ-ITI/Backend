using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class ProviderTrustScoreHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProviderProfileId { get; set; }

    public decimal TrustScore { get; set; }

    public TrustLevel TrustLevel { get; set; }

    public decimal? ComponentRating { get; set; }

    public decimal? ComponentCompletion { get; set; }

    public decimal? ComponentResponse { get; set; }

    public decimal? ComponentDocs { get; set; }

    public decimal? ComponentIncident { get; set; }

    public string? TriggerEvent { get; set; }

    public DateTime RecordedAt { get; set; }
    public ProviderProfile ProviderProfile { get; set; } = null!;

}
