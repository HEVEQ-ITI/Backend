namespace HEVEQ.Application.Features.ProviderEarnings.DTOs;

public record ProviderEarningsServiceSummaryDto(
    decimal GrossAmount,
    decimal PlatformCommission,
    decimal ProviderPayout,
    decimal HeldAmount,
    decimal ReleasedAmount,
    int CompletedBookingsCount
);