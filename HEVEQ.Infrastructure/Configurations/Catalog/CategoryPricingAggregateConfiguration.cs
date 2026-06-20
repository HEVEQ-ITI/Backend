using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class CategoryPricingAggregateConfiguration : IEntityTypeConfiguration<CategoryPricingAggregate>
{
    public void Configure(EntityTypeBuilder<CategoryPricingAggregate> builder)
    {
        builder.ToTable("CategoryPricingAggregates");
        builder.HasKey(x => new { x.CategoryId, x.LocationGovernorate, x.PriceType });

        builder.Property(x => x.LocationGovernorate).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MedianPrice).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Percentile25).HasColumnType("decimal(10,2)");
        builder.Property(x => x.Percentile75).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MinPrice).HasColumnType("decimal(10,2)");
        builder.Property(x => x.MaxPrice).HasColumnType("decimal(10,2)");
        builder.Property(x => x.ComputedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Category)
            .WithMany(x => x.PricingAggregates)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
