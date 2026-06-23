using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CancelBooking
{
    public sealed class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
    {
        public CancelBookingCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");

            RuleFor(x => x.reason)
                .NotEmpty()
                .WithMessage("Cancellation reason is required.")
                .MaximumLength(500)
                .WithMessage("Cancellation reason cannot exceed 500 characters.");
        }
    }
}
