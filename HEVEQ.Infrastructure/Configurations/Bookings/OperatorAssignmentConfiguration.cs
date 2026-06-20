using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Bookings;

public class OperatorAssignmentConfiguration : IEntityTypeConfiguration<OperatorAssignment>
{
    public void Configure(EntityTypeBuilder<OperatorAssignment> builder)
    {
        builder.ToTable("OperatorAssignments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Operator)
            .WithMany(x => x.OperatorAssignments)
            .HasForeignKey(x => x.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.OperatorAssignments)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.OperatorId, x.ScheduledStart, x.ScheduledEnd });
        builder.HasIndex(x => x.BookingId);
    }
}
