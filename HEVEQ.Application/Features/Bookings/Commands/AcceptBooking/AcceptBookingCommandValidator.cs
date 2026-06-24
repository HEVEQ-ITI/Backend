using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.AcceptBooking
{
    public sealed class AcceptBookingCommandValidator : AbstractValidator<AcceptBookingCommand>
    {
        public AcceptBookingCommandValidator()
        {
            RuleFor(x => x.ProviderUserId)
                .NotEmpty().WithMessage("ProviderId is required.");
            RuleFor(x => x.OperatorId)
                .NotEmpty().WithMessage("OperatorId is required.");
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("BookingId is required.");
        }
    }
}
