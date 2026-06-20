using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Platform;

public class PlatformSettingConfiguration : IEntityTypeConfiguration<PlatformSetting>
{
    public void Configure(EntityTypeBuilder<PlatformSetting> builder)
    {
        builder.ToTable("PlatformSettings");
        builder.HasKey(x => x.SettingKey);

        builder.Property(x => x.SettingKey).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SettingValue).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.UpdatedByAdmin)
            .WithMany(x => x.PlatformSettingsUpdated)
            .HasForeignKey(x => x.UpdatedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
