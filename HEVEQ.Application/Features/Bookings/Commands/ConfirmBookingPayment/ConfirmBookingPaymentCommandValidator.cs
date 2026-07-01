using FluentValidation;
namespace HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingPayment
{
    public sealed class ConfirmBookingPaymentCommandValidator : AbstractValidator<ConfirmBookingPaymentCommand>
    {
        public ConfirmBookingPaymentCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");
        }
    }
}