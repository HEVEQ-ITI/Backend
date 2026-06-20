using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Tickets;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TicketNumber).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Subject).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Priority).HasDefaultValue(0);
        builder.Property(x => x.AiSummary);
        builder.Property(x => x.AiIdentifiedIssue).HasMaxLength(500);
        builder.Property(x => x.AiClaimedImpact).HasMaxLength(500);
        builder.Property(x => x.AdminResolution);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.SubmittedBy)
            .WithMany(x => x.SubmittedTickets)
            .HasForeignKey(x => x.SubmittedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssignedToUser)
            .WithMany(x => x.AssignedTickets)
            .HasForeignKey(x => x.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ResolvedByUser)
            .WithMany(x => x.ResolvedTickets)
            .HasForeignKey(x => x.ResolvedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.EscalatedToAdmin)
            .WithMany(x => x.EscalatedTickets)
            .HasForeignKey(x => x.EscalatedToAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MarketplaceOrder)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.MarketplaceOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.TicketNumber).IsUnique();
        builder.HasIndex(x => new { x.Status, x.Priority });
        builder.HasIndex(x => x.SubmittedById);
        builder.HasIndex(x => x.AssignedToUserId);
    }
}
