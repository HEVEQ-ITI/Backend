using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Evidence;

public class FieldVerificationFormConfiguration : IEntityTypeConfiguration<FieldVerificationForm>
{
    public void Configure(EntityTypeBuilder<FieldVerificationForm> builder)
    {
        builder.ToTable("FieldVerificationForms");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DispatchInstructions).HasMaxLength(1000);
        builder.Property(x => x.EmployeeNotes);
        builder.Property(x => x.AiSimilarityScore).HasColumnType("decimal(5,2)");
        builder.Property(x => x.AiSimilarityNotes).HasMaxLength(1000);
        builder.Property(x => x.AdminDecisionNote).HasMaxLength(1000);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.FieldVerificationForms)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DispatchedEmployee)
            .WithMany(x => x.FieldVerificationFormsDispatched)
            .HasForeignKey(x => x.DispatchedEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DispatchedByAdmin)
            .WithMany(x => x.FieldVerificationFormsDispatchedByAdmin)
            .HasForeignKey(x => x.DispatchedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.DecidedByAdmin)
            .WithMany(x => x.FieldVerificationFormsDecidedByAdmin)
            .HasForeignKey(x => x.DecidedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LinkedEvidenceForm)
            .WithMany(x => x.FieldVerificationForms)
            .HasForeignKey(x => x.LinkedEvidenceFormId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.BookingId);
        builder.HasIndex(x => x.DispatchedEmployeeId);
    }
}
