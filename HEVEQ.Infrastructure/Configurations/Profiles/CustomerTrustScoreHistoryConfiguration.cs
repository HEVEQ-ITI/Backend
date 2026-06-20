using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Profiles;

public class CustomerTrustScoreHistoryConfiguration : IEntityTypeConfiguration<CustomerTrustScoreHistory>
{
    public void Configure(EntityTypeBuilder<CustomerTrustScoreHistory> builder)
    {
        builder.ToTable("CustomerTrustScoreHistory");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrustScore).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(x => x.TriggerEvent).HasMaxLength(50);
        builder.Property(x => x.RecordedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.CustomerProfile)
            .WithMany(x => x.TrustScoreHistory)
            .HasForeignKey(x => x.CustomerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CustomerProfileId, x.RecordedAt });
    }
}
