using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Catalog;

public class BlackoutDateConfiguration : IEntityTypeConfiguration<BlackoutDate>
{
    public void Configure(EntityTypeBuilder<BlackoutDate> builder)
    {
        builder.ToTable("BlackoutDates");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date).HasColumnType("date").HasColumnName("Date");
        builder.Property(x => x.Reason).HasMaxLength(300);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Listing)
            .WithMany(x => x.BlackoutDates)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Operator)
            .WithMany(x => x.BlackoutDates)
            .HasForeignKey(x => x.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ListingId, x.Date });
    }
}
