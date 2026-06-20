using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Marketplace;

public class MarketplaceOrderConfiguration : IEntityTypeConfiguration<MarketplaceOrder>
{
    public void Configure(EntityTypeBuilder<MarketplaceOrder> builder)
    {
        builder.ToTable("MarketplaceOrders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.DeliveryAddress).HasMaxLength(500);
        builder.Property(x => x.TrackingNumber).HasMaxLength(100);
        builder.Property(x => x.CancellationReason).HasMaxLength(500);
        builder.Property(x => x.ReturnShippingCost).HasColumnType("decimal(10,2)");
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Buyer)
            .WithMany(x => x.MarketplaceOrdersAsBuyer)
            .HasForeignKey(x => x.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.BuyerId, x.Status });
        builder.HasIndex(x => new { x.ListingId, x.Status });
    }
}
