using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class OperatorConfiguration : IEntityTypeConfiguration<Operator>
{
    public void Configure(EntityTypeBuilder<Operator> builder)
    {
        builder.ToTable("Operators");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Specialization).HasMaxLength(200);
        builder.Property(x => x.LicenseType).HasMaxLength(100);
        builder.Property(x => x.LicenseNumber).HasMaxLength(100);
        builder.Property(x => x.LicenseExpiryDate).HasColumnType("date");
        builder.Property(x => x.ProfilePhotoUrl).HasMaxLength(500);
        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.ProviderProfile)
            .WithMany(x => x.Operators)
            .HasForeignKey(x => x.ProviderProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ProviderProfileId, x.IsActive });
    }
}
