using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Domain.Entities;

namespace HEVEQ.Application.Features.Bookings.Services
{
    public static class BookingDtoMapper
    {
        public static BookingDto ToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                ServiceListingId = booking.ServiceListingId,
                ServiceListingTitle = booking.ServiceListing?.Title ?? string.Empty,
                CustomerId = booking.CustomerId,
                JobTitle = booking.JobTitle,
                JobDescription = booking.JobDescription,
                Governorate = booking.Governorate,
                District = booking.District,
                Street = booking.Street,
                Latitude = booking.Latitude,
                Longitude = booking.Longitude,
                RequestedStartDate = booking.RequestedStartDate,
                RequestedStartTime = booking.RequestedStartTime,
                EstimatedDurationHours = booking.EstimatedDurationHours,
                HourlyRateSnapshot = booking.HourlyRateSnapshot,
                EstimatedTotal = booking.EstimatedTotal,
                SurchargeAmount = booking.SurchargeAmount,
                IsOutOfZoneBooking = booking.IsOutOfZoneBooking,
                OutOfZoneDistanceKm = booking.OutOfZoneDistanceKm,
                OutOfZoneSurchargeAmount = booking.OutOfZoneSurchargeAmount,
                OutOfZoneSurchargeAcceptedAt = booking.OutOfZoneSurchargeAcceptedAt,
                Status = booking.Status.ToString(),
                AssignedOperatorId = booking.AssignedOperatorId,
                ProviderRejectionReason = booking.ProviderRejectionReason,
                CancellationReason = booking.CancellationReason,
                CreatedAt = booking.CreatedAt,
                ConfirmedAt = booking.ConfirmedAt,
                PaymentCapturedAt = booking.PaymentCapturedAt,
                StartedAt = booking.StartedAt,
                CompletedMarkedAt = booking.CompletedMarkedAt,
                CompletionConfirmedAt = booking.CompletionConfirmedAt,
                DisputeOpenedAt = booking.DisputeOpenedAt
            };
        }
    }
}