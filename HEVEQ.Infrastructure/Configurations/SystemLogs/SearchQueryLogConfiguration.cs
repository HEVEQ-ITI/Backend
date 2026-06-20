using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.SystemLogs;

public class SearchQueryLogConfiguration : IEntityTypeConfiguration<SearchQueryLog>
{
    public void Configure(EntityTypeBuilder<SearchQueryLog> builder)
    {
        builder.ToTable("SearchQueryLogs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SessionId).HasMaxLength(100);
        builder.Property(x => x.RawQuery).HasMaxLength(500).IsRequired();
        builder.Property(x => x.HasLowConfidence).HasDefaultValue(false);
        builder.Property(x => x.HasZeroResults).HasDefaultValue(false);
        builder.Property(x => x.AlternativeSuggested).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.User)
            .WithMany(x => x.SearchQueryLogs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ContextDomain, x.SearchMode, x.CreatedAt });
    }
}
