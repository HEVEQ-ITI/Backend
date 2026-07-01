using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.PartialSettleBookingDispute
{
    public class PartialSettleBookingDisputeCommandValidator : AbstractValidator<PartialSettleBookingDisputeCommand>
    {
        public PartialSettleBookingDisputeCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty();

            RuleFor(x => x.CustomerAmount).GreaterThanOrEqualTo(0)
                .WithMessage("Customer amount cannot be negative.");

            RuleFor(x => x.ProviderAmount).GreaterThanOrEqualTo(0)
                .WithMessage("Provider amount cannot be negative.");

            RuleFor(x => x.DecisionNote).NotEmpty()
                .WithMessage("Decision note is required for partial settlement.");
        }
    }
}
