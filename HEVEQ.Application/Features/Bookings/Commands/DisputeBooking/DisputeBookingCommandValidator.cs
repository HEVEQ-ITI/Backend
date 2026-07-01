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
               .WithMessage("Dispute reason is required.")
               .MaximumLength(1000)
               .WithMessage("Dispute reason cannot exceed 1000 characters.");

            RuleFor(x => x.EvidencePhotoUrls)
                .Must(x => x == null || x.Count <= 10)
                .WithMessage("Maximum 10 dispute evidence photos are allowed.");

            RuleForEach(x => x.EvidencePhotoUrls)
                .NotEmpty()
                .WithMessage("Evidence photo URL cannot be empty.")
                .MaximumLength(500)
                .WithMessage("Evidence photo URL cannot exceed 500 characters.");
        }
    }
}
