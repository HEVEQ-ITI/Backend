namespace HEVEQ.Api.Requests.Payments
{
    public sealed class PaymentCheckoutRequest
    {
        public string PaymentMethod { get; init; } = "Card";
        public string? SuccessUrl { get; init; }
        public string? CancelUrl { get; init; }
    }
}