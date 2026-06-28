using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.ApproveServiceListing
{
    public class ApproveServiceListingCommandValidator : AbstractValidator<ApproveServiceListingCommand>
    {
        public ApproveServiceListingCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Service Listing ID is required.");
        }
    }
}
