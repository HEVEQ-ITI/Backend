using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.FcmToken)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(50);

        builder.HasIndex(x => x.NormalizedEmail)
            .HasDatabaseName("EmailIndex")
            .IsUnique()
            .HasFilter("[NormalizedEmail] IS NOT NULL");

        builder.HasIndex(x => x.PhoneNumber)
            .HasDatabaseName("IX_Users_PhoneNumber")
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
