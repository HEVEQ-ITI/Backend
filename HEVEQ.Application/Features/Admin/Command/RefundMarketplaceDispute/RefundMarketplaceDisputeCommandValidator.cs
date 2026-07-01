using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RefundMarketplaceDispute
{
    public class RefundMarketplaceDisputeCommandValidator : AbstractValidator<RefundMarketplaceDisputeCommand>
    {
        public RefundMarketplaceDisputeCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();

            RuleFor(x => x.DecisionNote)
                .NotEmpty().WithMessage("Decision note is required to refund a dispute.");
        }
    }
}
