using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class ServiceListingPhotoConfiguration : IEntityTypeConfiguration<ServiceListingPhoto>
{
    public void Configure(EntityTypeBuilder<ServiceListingPhoto> builder)
    {
        builder.ToTable("ServiceListingPhotos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhotoUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ListingId, x.DisplayOrder });
    }
}
