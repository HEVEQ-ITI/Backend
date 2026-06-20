namespace HEVEQ.Domain.Entities;

public class FieldVerificationPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid FieldVerificationFormId { get; set; }

    public string PhotoUrl { get; set; } = string.Empty;

    public string? Caption { get; set; }

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; }
    public FieldVerificationForm FieldVerificationForm { get; set; } = null!;

}
