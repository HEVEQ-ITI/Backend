using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.CreateTimeAdjustment
{
    public sealed class CreateTimeAdjustmentCommandValidator : AbstractValidator<CreateTimeAdjustmentCommand>
    {
        public CreateTimeAdjustmentCommandValidator()
        {
            RuleFor(x => x.ProviderUserId)
                .NotEmpty()
                .WithMessage("ProviderUserId is required.");

            RuleFor(x => x.BookingId)
                .NotEmpty()
                .WithMessage("BookingId is required.");

            RuleFor(x => x.RequestedAdditionalHrs)
                .GreaterThan(0)
                .WithMessage("Requested additional hours must be greater than 0.");
            //ممكن نظبط جزء الساعات لو مش عايزينه يزود عن حد معين

            RuleFor(x => x.ProviderNote)
                .NotEmpty()
                .WithMessage("ProviderNote is required.")
                .MaximumLength(500)
                .WithMessage("ProviderNote cannot exceed 500 characters.");
        }
    }
}
