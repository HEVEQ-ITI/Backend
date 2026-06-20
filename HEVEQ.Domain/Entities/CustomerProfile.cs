using HEVEQ.Domain.Identity;
namespace HEVEQ.Domain.Entities;

public class CustomerProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public decimal TrustScore { get; set; }

    public decimal? CancellationRate { get; set; }

    public decimal? DisputeFrequencyScore { get; set; }

    public int PaymentFailureCount { get; set; }

    public decimal? ReviewAuthenticityScore { get; set; }

    public bool RequiresAdditionalVerification { get; set; }

    public int TotalBookings { get; set; }

    public DateTime? TrustScoreLastComputedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public ICollection<CustomerTrustScoreHistory> TrustScoreHistory { get; set; } = new List<CustomerTrustScoreHistory>();

}
