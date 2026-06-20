using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Evidence;

public class JobCompletionEvidenceFormConfiguration : IEntityTypeConfiguration<JobCompletionEvidenceForm>
{
    public void Configure(EntityTypeBuilder<JobCompletionEvidenceForm> builder)
    {
        builder.ToTable("JobCompletionEvidenceForms");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProviderNotes);
        builder.Property(x => x.AdminReviewNote).HasMaxLength(1000);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.JobCompletionEvidenceForms)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SubmittedByUser)
            .WithMany(x => x.JobCompletionEvidenceFormsSubmitted)
            .HasForeignKey(x => x.SubmittedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReviewedByAdmin)
            .WithMany(x => x.JobCompletionEvidenceFormsReviewed)
            .HasForeignKey(x => x.ReviewedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.BookingId);
    }
}
