using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Payments;
using HEVEQ.Application.Features.Bookings.DTOs;
using HEVEQ.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HEVEQ.Application.Features.Bookings.Commands.CheckoutBookingPayment
{
    public sealed class CheckoutBookingPaymentCommandHandler : IRequestHandler<CheckoutBookingPaymentCommand, BookingPaymentCheckoutResponseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPaymentCheckoutService _paymentCheckoutService;
        public CheckoutBookingPaymentCommandHandler(IApplicationDbContext context, IPaymentCheckoutService paymentCheckoutService)
        {
            _context = context;
            _paymentCheckoutService = paymentCheckoutService;
        }

        public async Task<BookingPaymentCheckoutResponseDto> Handle(CheckoutBookingPaymentCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(x => x.Id == request.BookingId, cancellationToken);

            if (booking is null)
                throw new InvalidOperationException("Booking was not found.");

            if (booking.CustomerId != request.CustomerId)
                throw new UnauthorizedAccessException("Only the booking customer can pay for this booking.");

            if (booking.Status != BookingStatus.ConfirmedPendingPayment)
                throw new InvalidOperationException("Booking is not ready for payment.");

            if (booking.EstimatedTotal <= 0)
                throw new InvalidOperationException("Booking amount is invalid.");

            var checkout = await _paymentCheckoutService.CreateCheckoutAsync(
                new PaymentCheckoutCreateRequest
                {
                    ReferenceType = PaymentReferenceType.Booking,
                    ReferenceId = booking.Id,
                    ReferenceNumber = booking.BookingNumber,
                    PayingUserId = request.CustomerId,
                    Amount = booking.EstimatedTotal,
                    Currency = "EGP",
                    PaymentMethod = request.PaymentMethod,
                    SuccessUrl = request.SuccessUrl,
                    CancelUrl = request.CancelUrl
                },
                cancellationToken
            );

            return new BookingPaymentCheckoutResponseDto
            {
                BookingId = booking.Id,
                Amount = booking.EstimatedTotal,
                Currency = "EGP",
                PaymentProvider = checkout.PaymentProvider,
                CheckoutUrl = checkout.CheckoutUrl,
                Status = checkout.Status,
                PaymentGatewayReference = checkout.PaymentGatewayReference
            };
        }
    }
}