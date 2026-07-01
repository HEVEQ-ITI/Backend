using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.OpenMarketplaceOrderDispute
{
    public class OpenMarketplaceOrderDisputeCommandValidator:AbstractValidator<OpenMarketplaceOrderDisputeCommand>
    {
        public OpenMarketplaceOrderDisputeCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Request.Reason).NotEmpty().MaximumLength(1000);
            RuleForEach(x => x.Request.EvidencePhotoUrls)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .When(x => x.Request.EvidencePhotoUrls != null && x.Request.EvidencePhotoUrls.Count > 0)
                .WithMessage("Each evidence URL must be a valid absolute URL.");
        }
    }
}
