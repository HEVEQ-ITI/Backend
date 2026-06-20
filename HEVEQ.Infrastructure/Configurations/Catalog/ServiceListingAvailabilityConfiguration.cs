using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class ServiceListingAvailabilityConfiguration : IEntityTypeConfiguration<ServiceListingAvailability>
{
    public void Configure(EntityTypeBuilder<ServiceListingAvailability> builder)
    {
        builder.ToTable("ServiceListingAvailability");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OpenTime).HasColumnType("time");
        builder.Property(x => x.CloseTime).HasColumnType("time");

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.Availability)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ListingId, x.DayOfWeek }).IsUnique();
    }
}
