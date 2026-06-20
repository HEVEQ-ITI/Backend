namespace HEVEQ.Domain.Entities;

public class ServiceListingOperator
{
    public Guid ListingId { get; set; }

    public Guid OperatorId { get; set; }
    public ServiceListing Listing { get; set; } = null!;

    public Operator Operator { get; set; } = null!;

}
