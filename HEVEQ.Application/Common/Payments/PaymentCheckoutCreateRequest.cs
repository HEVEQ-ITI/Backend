namespace HEVEQ.Application.Common.Payments
{
    public class PaymentCheckoutCreateRequest
    {
        public PaymentReferenceType ReferenceType { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public Guid PayingUserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string PaymentMethod { get; set; } = "Card";
        public string? SuccessUrl { get; set; }
        public string? CancelUrl { get; set; }
    }
}