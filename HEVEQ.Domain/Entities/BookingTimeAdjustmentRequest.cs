using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class BookingTimeAdjustmentRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookingId { get; set; }

    public decimal RequestedAdditionalHrs { get; set; }

    public decimal AdditionalCostAmount { get; set; }

    public BookingTimeAdjustmentStatus Status { get; set; }

    public string? ProviderNote { get; set; }

    public DateTime? CustomerAcknowledgedAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public Booking Booking { get; set; } = null!;

    public ICollection<EscrowRecord> EscrowRecords { get; set; } = new List<EscrowRecord>();

}
