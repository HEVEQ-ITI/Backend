using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Chat;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content);
        builder.Property(x => x.AttachmentUrl).HasMaxLength(500);
        builder.Property(x => x.IsBlocked).HasDefaultValue(false);
        builder.Property(x => x.SentAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Sender)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.ConversationId, x.SentAt });
        builder.HasIndex(x => x.SenderId);
    }
}
