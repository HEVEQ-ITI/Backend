using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.RejectDocument
{
    public class RejectDocumentCommandValidator:AbstractValidator<RejectDocumentCommand>
    {
        public RejectDocumentCommandValidator()
        {

            RuleFor(x => x.DocumentId).NotEmpty();
            RuleFor(x => x.Request.Reason)
                .NotEmpty().WithMessage("A rejection reason is required.")
                .MaximumLength(500);
        }
    }
}
