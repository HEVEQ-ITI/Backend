using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class ProviderIncident
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProviderProfileId { get; set; }

    public Guid? BookingId { get; set; }

    public ProviderIncidentType IncidentType { get; set; }

    public bool PenaltyApplied { get; set; }

    public string? AdminNote { get; set; }

    public DateTime OccurredAt { get; set; }
    public ProviderProfile ProviderProfile { get; set; } = null!;

    public Booking? Booking { get; set; }

}
