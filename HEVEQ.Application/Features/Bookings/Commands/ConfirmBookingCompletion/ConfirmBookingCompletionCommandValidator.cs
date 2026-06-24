using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.ConfirmBookingCompletion
{
    public sealed class ConfirmBookingCompletionCommandValidator : AbstractValidator<ConfirmBookingCompletionCommand>
    {
        public ConfirmBookingCompletionCommandValidator()
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
