using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Profiles;

public class ProviderProfileConfiguration : IEntityTypeConfiguration<ProviderProfile>
{
    public void Configure(EntityTypeBuilder<ProviderProfile> builder)
    {
        builder.ToTable("ProviderProfiles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.BusinessDescription).HasMaxLength(1000);
        builder.Property(x => x.BaseLatitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.BaseLongitude).HasColumnType("decimal(10,7)");
        builder.Property(x => x.ServiceZoneCenter).HasColumnType("geography");
        builder.Property(x => x.ServiceZonePoly).HasColumnType("geography");
        builder.Property(x => x.ServiceRadiusKm).HasDefaultValue(0);
        builder.Property(x => x.OnboardingTier).HasDefaultValue(0);
        builder.Property(x => x.AverageRating).HasColumnType("decimal(5,2)").HasDefaultValue(0m);
        builder.Property(x => x.ResponseRate).HasColumnType("decimal(5,4)").HasDefaultValue(0m);
        builder.Property(x => x.SearchRankingModifier).HasColumnType("decimal(5,4)").HasDefaultValue(0m);
        builder.Property(x => x.TrustScore).HasColumnType("decimal(5,2)").HasDefaultValue(0m);
        builder.Property(x => x.TrustLevel)
            .HasConversion<int>()
            .HasDefaultValue(TrustLevel.Standard);
        builder.Property(x => x.TotalReviewsCount).HasDefaultValue(0);
        builder.Property(x => x.CompletedBookingsCount).HasDefaultValue(0);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.User)
            .WithOne(x => x.ProviderProfile)
            .HasForeignKey<ProviderProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasIndex(x => new { x.ResponseRate, x.AverageRating, x.OnboardingTier });
    }
}
