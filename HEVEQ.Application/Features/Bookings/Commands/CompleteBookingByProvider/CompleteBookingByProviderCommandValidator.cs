using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CompleteBookingByProvider
{
    public sealed class CompleteBookingByProviderCommandValidator : AbstractValidator<CompleteBookingByProviderCommand>
    {
        public CompleteBookingByProviderCommandValidator()
        {
            RuleFor(x => x.ProviderUserId)
                .NotEmpty()
                .WithMessage("ProviderUserId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");

            RuleFor(x => x.Photos)
                .Must(p => p != null && p.Count >= 1)
                    .WithMessage("At least 1 completion photo is required.")
                .Must(p => p == null || p.Count <= 10)
                    .WithMessage("Maximum 10 completion photos are allowed.");
        }
    }
}
