using MediatR;

namespace HEVEQ.Application.Features.ProviderDashboard.Queries.GetProviderDashboardSummary;


public record ProviderDashboardSummaryDto(
    int TotalServiceListings,
    int ApprovedServiceListings,
    int PendingServiceListings,
    int PendingBookingRequests,
    int ActiveJobs,
    int CompletedJobs,
    decimal AverageRating,
    decimal ResponseRate,
    decimal TrustScore,
    string TrustLevel,
    decimal EarningsThisMonth);