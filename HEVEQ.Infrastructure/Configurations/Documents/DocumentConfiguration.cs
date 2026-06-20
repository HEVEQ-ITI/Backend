using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Documents;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.ConfidenceScore).HasColumnType("decimal(5,4)");
        builder.Property(x => x.ExpiryDate).HasColumnType("date");
        builder.Property(x => x.FailureReason).HasMaxLength(500);
        builder.Property(x => x.UploadedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.User)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceListing)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.ServiceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MarketplaceListing)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.MarketplaceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Operator)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.ServiceListingId);
        builder.HasIndex(x => x.MarketplaceListingId);
        builder.HasIndex(x => x.OperatorId);
    }
}
