using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Bookings.Commands.RejectTimeAdjustment
{
    public sealed class RejectTimeAdjustmentCommandValidator : AbstractValidator<RejectTimeAdjustmentCommand>
    {
        public RejectTimeAdjustmentCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("CustomerId is required.");

            RuleFor(x => x.TimeAdjustmentRequestId)
                .NotEmpty()
                .WithMessage("TimeAdjustmentRequestId is required.");
        }
    }
}
