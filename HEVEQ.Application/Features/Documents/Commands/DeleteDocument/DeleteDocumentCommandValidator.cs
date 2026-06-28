using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommandValidator:AbstractValidator<DeleteDocumentCommand>
    {
        public DeleteDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentId).NotEmpty();
        }
    }
}
