using HEVEQ.Domain.Enums;

namespace HEVEQ.Application.Features.ProviderProfiles.DTOs;

public class ProviderProfileCardDto
{
    public Guid ProviderProfileId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public decimal AverageRating { get; set; }
    public int TotalReviewsCount { get; set; }
    public int CompletedBookingsCount { get; set; }
    public decimal TrustScore { get; set; }
    public TrustLevel TrustLevel { get; set; }
    public DateTime ActiveSince { get; set; }
}