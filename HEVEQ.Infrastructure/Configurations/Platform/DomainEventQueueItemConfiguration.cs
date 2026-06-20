using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Platform;

public class DomainEventQueueItemConfiguration : IEntityTypeConfiguration<DomainEventQueueItem>
{
    public void Configure(EntityTypeBuilder<DomainEventQueueItem> builder)
    {
        builder.ToTable("DomainEventQueue");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType).HasMaxLength(200).IsRequired();
        builder.Property(x => x.EntityType).HasMaxLength(100).IsRequired();
        builder.Property(x => x.RetryCount).HasDefaultValue(0);
        builder.Property(x => x.FailureReason).HasMaxLength(1000);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(x => new { x.Status, x.CreatedAt });
    }
}
