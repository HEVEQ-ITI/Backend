using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingPayment
{
    public sealed record ConfirmBookingPaymentCommand(Guid CustomerId, Guid BookingId, string? PaymentGatewayReference) : IRequest<BookingPaymentConfirmResponseDto>;
}