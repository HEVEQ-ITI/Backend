namespace HEVEQ.Application.Features.Operators.DTOs;

public class OperatorDto
{
    public Guid Id { get; set; }
    public Guid ProviderProfileId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int? YearsOfExperience { get; set; }
    public string? Specialization { get; set; }
    public string? LicenseType { get; set; }
    public string? LicenseNumber { get; set; }
    public DateOnly? LicenseExpiryDate { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}