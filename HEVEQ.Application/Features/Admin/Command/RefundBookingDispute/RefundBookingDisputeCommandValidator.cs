using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RefundBookingDispute
{
    public class RefundBookingDisputeCommandValidator : AbstractValidator<RefundBookingDisputeCommand>
    {
        public RefundBookingDisputeCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty();

            RuleFor(x => x.DecisionNote)
                .NotEmpty().WithMessage("Decision note is required to refund a dispute.");
        }
    }
}
