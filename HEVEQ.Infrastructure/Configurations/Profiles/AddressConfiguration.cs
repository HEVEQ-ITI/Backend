using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Profiles;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Label).HasMaxLength(100);
        builder.Property(x => x.Governorate).HasMaxLength(100).IsRequired();
        builder.Property(x => x.District).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Street).HasMaxLength(300);
        builder.Property(x => x.Latitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.Longitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.IsDefault).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.User)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId);
    }
}
