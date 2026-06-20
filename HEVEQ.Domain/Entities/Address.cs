using HEVEQ.Domain.Identity;
namespace HEVEQ.Domain.Entities;

public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string? Label { get; set; }

    public string Governorate { get; set; } = string.Empty;

    public string District { get; set; } = string.Empty;

    public string? Street { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;

}
