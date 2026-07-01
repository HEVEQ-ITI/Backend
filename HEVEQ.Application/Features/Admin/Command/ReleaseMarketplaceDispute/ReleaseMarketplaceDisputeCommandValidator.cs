using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ReleaseMarketplaceDispute
{
    public class ReleaseMarketplaceDisputeCommandValidator : AbstractValidator<ReleaseMarketplaceDisputeCommand>
    {
        public ReleaseMarketplaceDisputeCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();

            RuleFor(x => x.DecisionNote)
                .NotEmpty().WithMessage("Decision note is required to resolve a dispute.");
        }
    }
}
