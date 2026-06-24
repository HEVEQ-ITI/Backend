using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CancelMarketplaceOrder
{
    public class CancelMarketplaceOrderCommandValidator:AbstractValidator<CancelMarketplaceOrderCommand>
    {
        public CancelMarketplaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Request.Reason)
                .MaximumLength(500)
                .When(x => x.Request.Reason is not null);
        }
    }
}
