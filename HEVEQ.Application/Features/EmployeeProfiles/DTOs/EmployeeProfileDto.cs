namespace HEVEQ.Application.Features.EmployeeProfiles.DTOs;

public class EmployeeProfileDto
{
    // From ApplicationUser
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    // From EmployeeProfile
    public Guid Id { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? AssignedGovernorate { get; set; }
    public bool IsAvailableForDispatch { get; set; }

    // Read-only system fields
    public int TotalVerificationsCompleted { get; set; }
    public int TotalTicketsHandled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}