using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class ServiceListingOperatorConfiguration : IEntityTypeConfiguration<ServiceListingOperator>
{
    public void Configure(EntityTypeBuilder<ServiceListingOperator> builder)
    {
        builder.ToTable("ServiceListingOperators");
        builder.HasKey(x => new { x.ListingId, x.OperatorId });

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.ServiceListingOperators)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Operator)
            .WithMany(x => x.ServiceListingOperators)
            .HasForeignKey(x => x.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ListingId);
        builder.HasIndex(x => x.OperatorId);
    }
}
