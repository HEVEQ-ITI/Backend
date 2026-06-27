namespace HEVEQ.Api.Requests.Bookings
{
    public sealed class CreateTimeAdjustmentRequest
    {
        public decimal AdditionalHours { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}