namespace HEVEQ.Domain.Entities;

public class MarketplaceListingPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ListingId { get; set; }

    public string PhotoUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public MarketplaceListing Listing { get; set; } = null!;

}
