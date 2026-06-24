using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.StartBooking
{
    public sealed class StartBookingCommandValidator : AbstractValidator<StartBookingCommand>
    {
        public StartBookingCommandValidator()
        {
            RuleFor(x => x.ProviderUserId)
                .NotEmpty()
                .WithMessage("ProviderUserId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");
        }
    }
}
