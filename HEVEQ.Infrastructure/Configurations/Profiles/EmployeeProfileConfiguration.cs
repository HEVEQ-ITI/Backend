using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Profiles;

public class EmployeeProfileConfiguration : IEntityTypeConfiguration<EmployeeProfile>
{
    public void Configure(EntityTypeBuilder<EmployeeProfile> builder)
    {
        builder.ToTable("EmployeeProfiles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EmployeeCode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Department).HasMaxLength(100);
        builder.Property(x => x.AssignedGovernorate).HasMaxLength(100);
        builder.Property(x => x.IsAvailableForDispatch).HasDefaultValue(false);
        builder.Property(x => x.TotalVerificationsCompleted).HasDefaultValue(0);
        builder.Property(x => x.TotalTicketsHandled).HasDefaultValue(0);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.User)
            .WithOne(x => x.EmployeeProfile)
            .HasForeignKey<EmployeeProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserId).IsUnique();
        builder.HasIndex(x => x.EmployeeCode).IsUnique();
        builder.HasIndex(x => new { x.IsAvailableForDispatch, x.AssignedGovernorate });
    }
}
