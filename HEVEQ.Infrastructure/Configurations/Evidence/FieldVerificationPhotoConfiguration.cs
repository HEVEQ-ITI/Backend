using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Evidence;

public class FieldVerificationPhotoConfiguration : IEntityTypeConfiguration<FieldVerificationPhoto>
{
    public void Configure(EntityTypeBuilder<FieldVerificationPhoto> builder)
    {
        builder.ToTable("FieldVerificationPhotos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhotoUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Caption).HasMaxLength(300);
        builder.Property(x => x.DisplayOrder).HasDefaultValue(0);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.FieldVerificationForm)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.FieldVerificationFormId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.FieldVerificationFormId, x.DisplayOrder });
    }
}
