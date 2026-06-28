namespace HEVEQ.Api.Requests.Bookings
{
    public sealed class DisputeBookingRequest
    {
        public string Reason { get; init; } = string.Empty;
        public IReadOnlyList<string> EvidencePhotoUrls { get; init; } = Array.Empty<string>();
    }
}