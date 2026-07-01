using FluentValidation;

namespace HEVEQ.Application.Features.Bookings.Commands.CheckoutBookingPayment
{
    public sealed class CheckoutBookingPaymentCommandValidator : AbstractValidator<CheckoutBookingPaymentCommand>
    {
        public CheckoutBookingPaymentCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .MaximumLength(50)
                .WithMessage("Payment method cannot exceed 50 characters.");

            RuleFor(x => x.SuccessUrl)
                .MaximumLength(500)
                .WithMessage("SuccessUrl cannot exceed 500 characters.");

            RuleFor(x => x.CancelUrl)
                .MaximumLength(500)
                .WithMessage("CancelUrl cannot exceed 500 characters.");
        }
    }
}