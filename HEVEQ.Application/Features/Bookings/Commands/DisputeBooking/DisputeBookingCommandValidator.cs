using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.DisputeBooking
{
    public sealed class DisputeBookingCommandValidator : AbstractValidator<DisputeBookingCommand>
    {
        public DisputeBookingCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(1000);

            RuleForEach(x => x.EvidencePhotoUrls)
                .MaximumLength(500);
        }
    }
}
