using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Profiles;

public class ProviderTrustScoreHistoryConfiguration : IEntityTypeConfiguration<ProviderTrustScoreHistory>
{
    public void Configure(EntityTypeBuilder<ProviderTrustScoreHistory> builder)
    {
        builder.ToTable("ProviderTrustScoreHistory");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrustScore).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ComponentRating).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ComponentCompletion).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ComponentResponse).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ComponentDocs).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ComponentIncident).HasColumnType("decimal(5,2)");
        builder.Property(x => x.TriggerEvent).HasMaxLength(50);
        builder.Property(x => x.RecordedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.ProviderProfile)
            .WithMany(x => x.TrustScoreHistory)
            .HasForeignKey(x => x.ProviderProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ProviderProfileId, x.RecordedAt });
    }
}
