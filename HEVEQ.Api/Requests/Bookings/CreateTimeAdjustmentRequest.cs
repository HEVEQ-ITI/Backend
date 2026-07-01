namespace HEVEQ.Api.Requests.Bookings
{
    public sealed class CreateTimeAdjustmentRequest
    {
        public decimal RequestedAdditionalHrs { get; init; }
        public string ProviderNote { get; init; } = string.Empty;
    }
}