namespace HEVEQ.Domain.Entities;

public class ServiceListingAvailability
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ListingId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }
    public ServiceListing Listing { get; set; } = null!;

}
