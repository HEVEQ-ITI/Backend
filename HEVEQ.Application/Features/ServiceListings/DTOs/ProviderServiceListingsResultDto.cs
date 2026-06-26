namespace HEVEQ.Application.Features.ServiceListings.DTOs;

public class ProviderServiceListingsResultDto
{
    public List<ProviderServiceListingDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
}