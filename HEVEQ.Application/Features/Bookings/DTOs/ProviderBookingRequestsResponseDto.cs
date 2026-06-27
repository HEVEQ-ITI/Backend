namespace HEVEQ.Application.Features.Bookings.DTOs
{
    public class ProviderBookingRequestsResponseDto
    {
        public IReadOnlyList<ProviderBookingRequestItemDto> Items { get; set; } = new List<ProviderBookingRequestItemDto>();
        public int TotalCount { get; set; }
    }
}