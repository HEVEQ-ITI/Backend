using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.ApproveDocument
{
    public class ApproveDocumentCommandValidator:AbstractValidator<ApproveDocumentCommand>
    {
        public ApproveDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentId).NotEmpty();
        }
    }
}
