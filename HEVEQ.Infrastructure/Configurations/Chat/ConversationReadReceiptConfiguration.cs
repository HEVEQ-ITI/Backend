using HEVEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HEVEQ.Infrastructure.Configurations.Chat;

public class ConversationReadReceiptConfiguration : IEntityTypeConfiguration<ConversationReadReceipt>
{
    public void Configure(EntityTypeBuilder<ConversationReadReceipt> builder)
    {
        builder.ToTable("ConversationReadReceipts");
        builder.HasKey(x => new { x.ConversationId, x.UserId });

        builder.Property(x => x.LastReadAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Conversation)
            .WithMany(x => x.ReadReceipts)
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.ConversationReadReceipts)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LastReadMessage)
            .WithMany(x => x.ReadReceiptsAsLastRead)
            .HasForeignKey(x => x.LastReadMessageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
