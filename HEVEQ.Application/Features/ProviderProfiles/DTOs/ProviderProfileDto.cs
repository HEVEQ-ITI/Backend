using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.ProviderProfiles.DTOs;

public class ProviderProfileDto
{
    // From ApplicationUser — editable
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    // From ProviderProfile — editable
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string? BusinessDescription { get; set; }
    public decimal? BaseLatitude { get; set; }
    public decimal? BaseLongitude { get; set; }
    public int ServiceRadiusKm { get; set; }

    // From ProviderProfile — READ ONLY, system computed
    public decimal AverageRating { get; set; }
    public int TotalReviewsCount { get; set; }
    public int CompletedBookingsCount { get; set; }
    public decimal ResponseRate { get; set; }
    public decimal TrustScore { get; set; }
    public TrustLevel TrustLevel { get; set; }
    public int OnboardingTier { get; set; }
    public decimal SearchRankingModifier { get; set; }
    public DateTime? TrustScoreLastComputedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}