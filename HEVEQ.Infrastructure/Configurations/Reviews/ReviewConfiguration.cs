using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Reviews;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Comment).HasMaxLength(1000);
        builder.Property(x => x.IsPublished).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Reviewer)
            .WithMany(x => x.ReviewsWritten)
            .HasForeignKey(x => x.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReviewedUser)
            .WithMany(x => x.ReviewsReceived)
            .HasForeignKey(x => x.ReviewedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceListing)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.ServiceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MarketplaceOrder)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.MarketplaceOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MarketplaceListing)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.MarketplaceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ReviewerId);
        builder.HasIndex(x => x.ReviewedUserId);
    }
}
