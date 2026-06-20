using HEVEQ.Domain.Identity;
namespace HEVEQ.Domain.Entities;

public class EmployeeProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public string EmployeeCode { get; set; } = string.Empty;

    public string? Department { get; set; }

    public string? AssignedGovernorate { get; set; }

    public bool IsAvailableForDispatch { get; set; }

    public int TotalVerificationsCompleted { get; set; }

    public int TotalTicketsHandled { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;

}
