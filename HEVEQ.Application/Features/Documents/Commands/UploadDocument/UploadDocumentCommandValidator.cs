using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.Documents.Commands.UploadDocument
{
    public class UploadDocumentCommandValidator:AbstractValidator<UploadDocumentCommand>
    {
        public UploadDocumentCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.Request.FileUrl)
                .NotEmpty()
                .MaximumLength(500)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("FileUrl must be a valid absolute URL.");

            RuleFor(x => x.Request.DocumentType).IsInEnum();

            RuleFor(x => x.Request.ExpiryDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .When(x => x.Request.ExpiryDate.HasValue)
                .WithMessage("ExpiryDate must be a future date.");
        }
    }
}
