namespace HEVEQ.Domain.Entities;

public class ServiceListingPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ListingId { get; set; }

    public string PhotoUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public ServiceListing Listing { get; set; } = null!;

}
