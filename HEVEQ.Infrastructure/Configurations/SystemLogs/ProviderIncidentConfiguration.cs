using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.SystemLogs;

public class ProviderIncidentConfiguration : IEntityTypeConfiguration<ProviderIncident>
{
    public void Configure(EntityTypeBuilder<ProviderIncident> builder)
    {
        builder.ToTable("ProviderIncidents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AdminNote).HasMaxLength(500);
        builder.Property(x => x.PenaltyApplied).HasDefaultValue(false);
        builder.Property(x => x.OccurredAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.ProviderProfile)
            .WithMany(x => x.Incidents)
            .HasForeignKey(x => x.ProviderProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.ProviderIncidents)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ProviderProfileId, x.OccurredAt });
    }
}
