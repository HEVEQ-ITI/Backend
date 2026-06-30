namespace HEVEQ.Application.Features.CustomerProfiles.DTOs;

public class AddressDto
{
    public Guid Id { get; set; }
    public string? Label { get; set; }
    public string Governorate { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string? Street { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsDefault { get; set; }

}