using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class OperatorAssignment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid OperatorId { get; set; }

    public Guid BookingId { get; set; }

    public DateTime ScheduledStart { get; set; }

    public DateTime ScheduledEnd { get; set; }

    public OperatorAssignmentStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
    public Operator Operator { get; set; } = null!;

    public Booking Booking { get; set; } = null!;

}
