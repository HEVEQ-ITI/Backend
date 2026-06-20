using HEVEQ.Domain.Enums;
using HEVEQ.Domain.Identity;

namespace HEVEQ.Domain.Entities;

public class TicketMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TicketId { get; set; }

    public Guid SenderId { get; set; }

    public string Body { get; set; } = string.Empty;

    public TicketMessageType MessageType { get; set; }

    public bool IsInternal { get; set; }

    public DateTime CreatedAt { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public ApplicationUser Sender { get; set; } = null!;

    public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();

}
