namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class ProviderActiveJobsResponseDto
    {
        public IReadOnlyList<ProviderActiveJobItemDto> Items { get; set; } = new List<ProviderActiveJobItemDto>();
    }
}