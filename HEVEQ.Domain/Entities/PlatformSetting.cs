using HEVEQ.Domain.Identity;
namespace HEVEQ.Domain.Entities;

public class PlatformSetting
{
    public string SettingKey { get; set; } = string.Empty;

    public string SettingValue { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid? UpdatedByAdminId { get; set; }

    public DateTime UpdatedAt { get; set; }
    public ApplicationUser? UpdatedByAdmin { get; set; }

}
