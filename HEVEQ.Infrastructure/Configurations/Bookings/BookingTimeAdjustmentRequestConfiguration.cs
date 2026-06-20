using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Bookings;

public class BookingTimeAdjustmentRequestConfiguration : IEntityTypeConfiguration<BookingTimeAdjustmentRequest>
{
    public void Configure(EntityTypeBuilder<BookingTimeAdjustmentRequest> builder)
    {
        builder.ToTable("BookingTimeAdjustmentRequests");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestedAdditionalHrs).HasColumnType("decimal(4,2)");
        builder.Property(x => x.AdditionalCostAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.ProviderNote).HasMaxLength(300);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.TimeAdjustmentRequests)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.BookingId, x.Status });
    }
}
