using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Bookings;

public class EscrowRecordConfiguration : IEntityTypeConfiguration<EscrowRecord>
{
    public void Configure(EntityTypeBuilder<EscrowRecord> builder)
    {
        builder.ToTable("EscrowRecords");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.GrossAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.PlatformCommission).HasColumnType("decimal(10,2)");
        builder.Property(x => x.CommissionRateSnapshot).HasColumnType("decimal(5,4)");
        builder.Property(x => x.ProviderPayout).HasColumnType("decimal(10,2)");
        builder.Property(x => x.VatAmount).HasColumnType("decimal(10,2)");
        builder.Property(x => x.PartialSettleCustomerAmt).HasColumnType("decimal(10,2)");
        builder.Property(x => x.PartialSettleProviderAmt).HasColumnType("decimal(10,2)");
        builder.Property(x => x.PaymentGatewayReference).HasMaxLength(200);
        builder.Property(x => x.AdditionalHoldDays).HasDefaultValue(0);
        builder.Property(x => x.FreezeReason).HasMaxLength(500);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.Timestamp).IsRowVersion();

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.EscrowRecords)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MarketplaceOrder)
            .WithMany(x => x.EscrowRecords)
            .HasForeignKey(x => x.MarketplaceOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AdjustmentRequest)
            .WithMany(x => x.EscrowRecords)
            .HasForeignKey(x => x.AdjustmentRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.BookingId).IsUnique().HasFilter("[BookingId] IS NOT NULL");
        builder.HasIndex(x => x.MarketplaceOrderId).IsUnique().HasFilter("[MarketplaceOrderId] IS NOT NULL");
    }
}
