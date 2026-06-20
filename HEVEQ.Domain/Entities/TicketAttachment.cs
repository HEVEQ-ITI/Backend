using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class TicketAttachment
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TicketMessageId { get; set; }

    public Guid UploadedByUserId { get; set; }

    public string FileUrl { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public AttachmentFileType FileType { get; set; }

    public DateTime CreatedAt { get; set; }
    public TicketMessage TicketMessage { get; set; } = null!;

    public ApplicationUser UploadedByUser { get; set; } = null!;

}
