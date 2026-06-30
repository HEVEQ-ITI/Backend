namespace HEVEQ.Application.Features.ProfileCompletion.DTOs;

public class ProfileCompletionContextDto
{
    // Role from JWT — "Customer" | "Provider" | "Employee"
    public string Role { get; set; } = string.Empty;

    // True if the user's role-specific profile record exists and has required fields
    public bool ProfileCompleted { get; set; }

    // From ApplicationUser.PhoneNumberConfirmed
    public bool PhoneVerified { get; set; }

    // True if at least one Address with IsDefault=true exists
    public bool HasDefaultAddress { get; set; }

    // Only populated for Provider role — null for Customer / Employee
    public bool? ProviderProfileCompleted { get; set; }

    // Angular uses this list to decide which step to show on the completion page
    // Empty list = user can proceed to dashboard
    public List<string> MissingRequirements { get; set; } = new();
}