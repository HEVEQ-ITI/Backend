using HEVEQ.Application.Features.Bookings.DTOs;
using MediatR;

namespace HEVEQ.Application.Features.Bookings.Commands.CheckoutBookingPayment
{
    public sealed record CheckoutBookingPaymentCommand(Guid CustomerId, Guid BookingId, string PaymentMethod, string? SuccessUrl, string? CancelUrl) : IRequest<BookingPaymentCheckoutResponseDto>;
}