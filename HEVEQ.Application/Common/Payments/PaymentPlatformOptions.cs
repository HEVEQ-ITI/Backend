namespace HEVEQ.Application.Common.Payments
{
    public class PaymentPlatformOptions
    {
        public decimal PlatformCommissionRate { get; set; } = 0.10m;
        public int DefaultEscrowHoldDays { get; set; } = 2;
    }
}