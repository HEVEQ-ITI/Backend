using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ReleaseBookingDispute
{
    public class ReleaseBookingDisputeCommandValidator : AbstractValidator<ReleaseBookingDisputeCommand>
    {
        public ReleaseBookingDisputeCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty();

            RuleFor(x => x.DecisionNote)
                .NotEmpty().WithMessage("Decision note is required to resolve a dispute.");
        }
    }
}
