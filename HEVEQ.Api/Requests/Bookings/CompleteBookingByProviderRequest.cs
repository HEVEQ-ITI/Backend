using HEVEQ.Application.Features.Bookings.DTOs;

namespace HEVEQ.Api.Requests.Bookings
{
    public sealed record CompleteBookingByProviderRequest(string? ProviderNotes, IReadOnlyList<CompletionEvidencePhotoDto> Photos);
}
