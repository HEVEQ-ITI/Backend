using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Admin.Command.RejectServiceListing
{
    public class RejectServiceListingCommandValidator : AbstractValidator<RejectServiceListingCommand>
    {
        public RejectServiceListingCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.AdminRejectionNote)
                .NotEmpty().WithMessage("Admin rejection note is required.")
                .MaximumLength(1000).WithMessage("Rejection note cannot exceed 1000 characters.");
        }
    }
}
