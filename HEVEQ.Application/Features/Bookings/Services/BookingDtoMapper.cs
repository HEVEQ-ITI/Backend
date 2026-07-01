using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Domain.Entities;
using HEVEQ.Application.Features.Bookings.Helpers;

namespace HEVEQ.Application.Features.Bookings.Services
{
    public static class BookingDtoMapper
    {
        public static BookingDto ToDto(Booking booking, string? role = null)
        {
            var isCustomer = string.Equals(role, "Customer", StringComparison.OrdinalIgnoreCase);
            var isProvider = string.Equals(role, "Provider", StringComparison.OrdinalIgnoreCase);
            var isAdmin = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
            var escrow = booking.EscrowRecords.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            var actions = new BookingActionsDto
            {
                CanAccept = isProvider && BookingActionsHelper.CanProviderAccept(booking.Status),
                CanReject = isProvider && BookingActionsHelper.CanProviderReject(booking.Status),
                CanStart = isProvider && BookingActionsHelper.CanProviderStart(booking.Status, booking.AssignedOperatorId),
                CanComplete = isProvider && BookingActionsHelper.CanProviderComplete(booking.Status),
                CanProviderCancel = isProvider && BookingActionsHelper.CanProviderCancel(booking.Status),
                CanConfirmCompletion = isCustomer && BookingActionsHelper.CanCustomerConfirmCompletion(booking.Status),
                CanDispute = isCustomer && BookingActionsHelper.CanCustomerDispute(booking.Status),
                CanCancel = isCustomer && BookingActionsHelper.CanCustomerCancel(booking.Status),
                CanPay = isCustomer && BookingActionsHelper.CanCustomerPay(booking.Status),
            };
            return new BookingDto
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,

                ServiceListingId = booking.ServiceListingId,
                ServiceTitle = booking.ServiceListing?.Title ?? string.Empty,

                CustomerId = booking.CustomerId,
                CustomerName = booking.Customer is null ? string.Empty : $"{booking.Customer.FirstName} {booking.Customer.LastName}".Trim(),

                ProviderCompany = booking.ServiceListing?.ProviderProfile?.CompanyName ?? string.Empty,

                AssignedOperatorId = booking.AssignedOperatorId,
                OperatorName = booking.AssignedOperator?.FullName,

                AvailableActions = actions,
                EscrowStatus = escrow?.Status.ToString(),
                EscrowStatusAr = escrow is null ? string.Empty : EscrowDisplayHelper.GetStatusAr(escrow.Status),

                Timeline = BookingTimelineHelper.Build(booking),

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
                StatusAr = BookingDisplayHelper.GetStatusAr(booking.Status),
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