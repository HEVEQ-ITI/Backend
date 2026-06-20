using HEVEQ.Domain.Entities;
using HEVEQ.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Marketplace;

public class MarketplaceListingConfiguration : IEntityTypeConfiguration<MarketplaceListing>
{
    public void Configure(EntityTypeBuilder<MarketplaceListing> builder)
    {
        builder.ToTable("MarketplaceListings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Specifications);
        builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
        builder.Property(x => x.IsNegotiable).HasDefaultValue(false);
        builder.Property(x => x.Governorate).HasMaxLength(100);
        builder.Property(x => x.District).HasMaxLength(100);
        builder.Property(x => x.AiRiskLevel).HasMaxLength(50);
        builder.Property(x => x.VideoUrl).HasMaxLength(500);
        builder.Property(x => x.QdrantPointId).HasMaxLength(100);
        builder.Property(x => x.AdminRejectionNote).HasMaxLength(500);
        builder.Property(x => x.SubmissionCount).HasDefaultValue(1);
        builder.Property(x => x.EmbeddingStatus)
            .HasConversion<int>()
            .HasDefaultValue(EmbeddingStatus.Pending);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Seller)
            .WithMany(x => x.MarketplaceListingsAsSeller)
            .HasForeignKey(x => x.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.MarketplaceListings)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RejectedByAdmin)
            .WithMany()
            .HasForeignKey(x => x.RejectedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.SellerId, x.Status });
        builder.HasIndex(x => new { x.CategoryId, x.Status });
    }
}
