using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Bookings;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.JobTitle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.JobDescription);
        builder.Property(x => x.Governorate).HasMaxLength(100).IsRequired();
        builder.Property(x => x.District).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Street).HasMaxLength(300);
        builder.Property(x => x.Latitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.Longitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.ServiceLocationGeo).HasColumnType("geography");
        builder.Property(x => x.SiteContactName).HasMaxLength(200);
        builder.Property(x => x.SiteContactPhone).HasMaxLength(50);
        builder.Property(x => x.AccessRequirements).HasMaxLength(1000);
        builder.Property(x => x.SafetyNotes).HasMaxLength(1000);
        builder.Property(x => x.RequestedStartDate).HasColumnType("date");
        builder.Property(x => x.RequestedStartTime).HasColumnType("time");
        builder.Property(x => x.EstimatedDurationHours).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ActualDurationHours).HasColumnType("decimal(5,2)");
        builder.Property(x => x.HourlyRateSnapshot).HasColumnType("decimal(10,2)");
        builder.Property(x => x.EstimatedTotal).HasColumnType("decimal(10,2)");
        builder.Property(x => x.SurchargeAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.IsOutOfZoneBooking).HasDefaultValue(false);
        builder.Property(x => x.OutOfZoneDistanceKm).HasColumnType("decimal(8,2)");
        builder.Property(x => x.OutOfZoneSurchargeAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.PrioritySurchargeAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.FuelSurchargeAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.ProviderRejectionReason).HasMaxLength(500);
        builder.Property(x => x.CancellationReason).HasMaxLength(500);
        builder.Property(x => x.CancellationRefundPct).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ProviderCancellationPenaltyApplied).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.Timestamp).IsRowVersion();

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.BookingsAsCustomer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceListing)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.ServiceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssignedOperator)
            .WithMany(x => x.Bookings)
            .HasForeignKey(x => x.AssignedOperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReassignedToBooking)
            .WithMany()
            .HasForeignKey(x => x.ReassignedToBookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.OriginalBooking)
            .WithMany(x => x.ReassignedFromBookings)
            .HasForeignKey(x => x.OriginalBookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.CustomerId);
        builder.HasIndex(x => new { x.ServiceListingId, x.Status });
        builder.HasIndex(x => x.AssignedOperatorId);
    }
}
