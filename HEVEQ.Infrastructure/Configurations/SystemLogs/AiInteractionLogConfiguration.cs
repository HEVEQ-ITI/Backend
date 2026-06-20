using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.SystemLogs;

public class AiInteractionLogConfiguration : IEntityTypeConfiguration<AiInteractionLog>
{
    public void Configure(EntityTypeBuilder<AiInteractionLog> builder)
    {
        builder.ToTable("AiInteractionLogs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InvocationContext).IsRequired();
        builder.Property(x => x.EntityType).HasMaxLength(100);
        builder.Property(x => x.AiRecommendation).HasMaxLength(100);
        builder.Property(x => x.AdminOverride).HasMaxLength(500);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.ExpiresAt).HasDefaultValueSql("DATEADD(day, 90, GETUTCDATE())");

        builder.HasOne(x => x.AdminOverrideBy)
            .WithMany(x => x.AiInteractionLogsAdminOverrides)
            .HasForeignKey(x => x.AdminOverrideById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.AgentType, x.CreatedAt });
        builder.HasIndex(x => new { x.EntityType, x.EntityId });
    }
}
