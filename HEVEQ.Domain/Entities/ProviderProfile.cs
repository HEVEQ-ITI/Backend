using HEVEQ.Domain.Enums;
using NetTopologySuite.Geometries;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class ProviderProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string? BusinessDescription { get; set; }

    public decimal? BaseLatitude { get; set; }

    public decimal? BaseLongitude { get; set; }

    public int ServiceRadiusKm { get; set; }

    public Point? ServiceZoneCenter { get; set; }

    public Geometry? ServiceZonePoly { get; set; }

    public int OnboardingTier { get; set; }

    public decimal AverageRating { get; set; }

    public int TotalReviewsCount { get; set; }

    public int CompletedBookingsCount { get; set; }

    public decimal ResponseRate { get; set; }

    public decimal SearchRankingModifier { get; set; }

    public decimal TrustScore { get; set; }

    public TrustLevel TrustLevel { get; set; }

    public DateTime? TrustScoreLastComputedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;

    public ICollection<Operator> Operators { get; set; } = new List<Operator>();

    public ICollection<ServiceListing> ServiceListings { get; set; } = new List<ServiceListing>();

    public ICollection<ProviderTrustScoreHistory> TrustScoreHistory { get; set; } = new List<ProviderTrustScoreHistory>();

    public ICollection<ProviderIncident> Incidents { get; set; } = new List<ProviderIncident>();

}
