using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Tickets;

public class TicketAttachmentConfiguration : IEntityTypeConfiguration<TicketAttachment>
{
    public void Configure(EntityTypeBuilder<TicketAttachment> builder)
    {
        builder.ToTable("TicketAttachments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.TicketMessage)
            .WithMany(x => x.Attachments)
            .HasForeignKey(x => x.TicketMessageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UploadedByUser)
            .WithMany(x => x.TicketAttachments)
            .HasForeignKey(x => x.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TicketMessageId);
    }
}
