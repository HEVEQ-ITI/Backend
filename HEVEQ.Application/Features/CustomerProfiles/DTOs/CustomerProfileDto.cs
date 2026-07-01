namespace HEVEQ.Application.Features.CustomerProfiles.DTOs;

public class CustomerProfileDto
{
    // From ApplicationUser — editable
    public Guid UserId { get; set; }
    public string DisplayName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPhoneVerified { get; set; }
    public AddressDto? DefaultAddress { get; set; }  // null if not set yet

    // From CustomerProfile — READ ONLY, system computed
    public Guid Id { get; set; }
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
}