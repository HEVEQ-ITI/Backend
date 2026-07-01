namespace HEVEQ.Api.Requests.Payments
{
    public sealed class PaymentConfirmRequest
    {
        public string? PaymentGatewayReference { get; init; }
    }
}