using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Profiles;

public class CustomerProfileConfiguration : IEntityTypeConfiguration<CustomerProfile>
{
    public void Configure(EntityTypeBuilder<CustomerProfile> builder)
    {
        builder.ToTable("CustomerProfiles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TrustScore).HasColumnType("decimal(5,2)").HasDefaultValue(100m);
        builder.Property(x => x.CancellationRate).HasColumnType("decimal(5,4)");
        builder.Property(x => x.DisputeFrequencyScore).HasColumnType("decimal(5,2)");
        builder.Property(x => x.ReviewAuthenticityScore).HasColumnType("decimal(5,2)");
        builder.Property(x => x.PaymentFailureCount).HasDefaultValue(0);
        builder.Property(x => x.RequiresAdditionalVerification).HasDefaultValue(false);
        builder.Property(x => x.TotalBookings).HasDefaultValue(0);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.User)
            .WithOne(x => x.CustomerProfile)
            .HasForeignKey<CustomerProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId).IsUnique();
    }
}
