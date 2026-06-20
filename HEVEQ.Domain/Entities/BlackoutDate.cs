namespace HEVEQ.Domain.Entities;

public class BlackoutDate
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ListingId { get; set; }

    public Guid? OperatorId { get; set; }

    public DateOnly Date { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; }
    public ServiceListing Listing { get; set; } = null!;

    public Operator? Operator { get; set; }

}
