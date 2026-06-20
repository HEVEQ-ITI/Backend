namespace HEVEQ.Domain.Entities;

public class Operator
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProviderProfileId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public int? YearsOfExperience { get; set; }

    public string? Specialization { get; set; }

    public string? LicenseType { get; set; }

    public string? LicenseNumber { get; set; }

    public DateOnly? LicenseExpiryDate { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
    public ProviderProfile ProviderProfile { get; set; } = null!;

    public ICollection<ServiceListingOperator> ServiceListingOperators { get; set; } = new List<ServiceListingOperator>();

    public ICollection<BlackoutDate> BlackoutDates { get; set; } = new List<BlackoutDate>();

    public ICollection<OperatorAssignment> OperatorAssignments { get; set; } = new List<OperatorAssignment>();

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

}
