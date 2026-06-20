using HEVEQ.Domain.Enums;

namespace HEVEQ.Domain.Entities;

public class EscrowRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid? BookingId { get; set; }

    public Guid? MarketplaceOrderId { get; set; }

    public Guid? AdjustmentRequestId { get; set; }

    public decimal GrossAmount { get; set; }

    public decimal PlatformCommission { get; set; }

    public decimal CommissionRateSnapshot { get; set; }

    public decimal ProviderPayout { get; set; }

    public decimal VatAmount { get; set; }

    public decimal? PartialSettleCustomerAmt { get; set; }

    public decimal? PartialSettleProviderAmt { get; set; }

    public EscrowStatus Status { get; set; }

    public string? PaymentGatewayReference { get; set; }

    public int AdditionalHoldDays { get; set; }

    public DateTime? EarliestReleaseAt { get; set; }

    public DateTime? CapturedAt { get; set; }

    public DateTime? HeldAt { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public DateTime? FrozenAt { get; set; }

    public string? FreezeReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public byte[] Timestamp { get; set; } = Array.Empty<byte>();
    public Booking? Booking { get; set; }

    public MarketplaceOrder? MarketplaceOrder { get; set; }

    public BookingTimeAdjustmentRequest? AdjustmentRequest { get; set; }

}
