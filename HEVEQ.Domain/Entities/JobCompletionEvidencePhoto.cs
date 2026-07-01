namespace HEVEQ.Domain.Entities;

public class JobCompletionEvidencePhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FormId { get; set; }
    public string PhotoUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    public JobCompletionEvidenceForm Form { get; set; } = null!;

}
