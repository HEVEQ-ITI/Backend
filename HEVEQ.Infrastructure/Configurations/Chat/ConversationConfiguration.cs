using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Chat;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("Conversations");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsLocked).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.ServiceListing)
            .WithMany(x => x.Conversations)
            .HasForeignKey(x => x.ServiceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MarketplaceListing)
            .WithMany(x => x.Conversations)
            .HasForeignKey(x => x.MarketplaceListingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Booking)
            .WithMany(x => x.Conversations)
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.InitiatedBy)
            .WithMany(x => x.ConversationsInitiated)
            .HasForeignKey(x => x.InitiatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Participant)
            .WithMany(x => x.ConversationsParticipated)
            .HasForeignKey(x => x.ParticipantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.InitiatedById);
        builder.HasIndex(x => x.ParticipantId);
        builder.HasIndex(x => x.BookingId);
    }
}
