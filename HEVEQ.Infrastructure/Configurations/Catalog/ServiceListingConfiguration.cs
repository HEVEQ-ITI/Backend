using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class ServiceListingConfiguration : IEntityTypeConfiguration<ServiceListing>
{
    public void Configure(EntityTypeBuilder<ServiceListing> builder)
    {
        builder.ToTable("ServiceListings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Tags).HasMaxLength(500);
        builder.Property(x => x.EquipmentModel).HasMaxLength(200);
        builder.Property(x => x.EquipmentCapacity).HasMaxLength(100);
        builder.Property(x => x.EquipmentRegistrationNumber).HasMaxLength(100);
        builder.Property(x => x.HourlyRate).HasColumnType("decimal(10,2)");
        builder.Property(x => x.DailyRate).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MinimumBookingHours).HasDefaultValue(1);
        builder.Property(x => x.AiRiskLevel).HasMaxLength(50);
        builder.Property(x => x.AiRecommendation).HasMaxLength(100);
        builder.Property(x => x.AdminRejectionNote).HasMaxLength(500);
        builder.Property(x => x.QdrantPointId).HasMaxLength(100);
        builder.Property(x => x.SubmissionCount).HasDefaultValue(1);
        builder.Property(x => x.EmbeddingStatus)
            .HasConversion<int>()
            .HasDefaultValue(EmbeddingStatus.Pending);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.ProviderProfile)
            .WithMany(x => x.ServiceListings)
            .HasForeignKey(x => x.ProviderProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.ServiceListings)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RejectedByAdmin)
            .WithMany()
            .HasForeignKey(x => x.RejectedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ProviderProfileId, x.Status });
        builder.HasIndex(x => new { x.CategoryId, x.Status });
    }
}
