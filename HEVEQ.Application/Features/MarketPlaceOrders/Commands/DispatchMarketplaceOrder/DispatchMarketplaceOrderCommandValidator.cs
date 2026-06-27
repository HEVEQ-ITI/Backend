using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DispatchMarketplaceOrder
{
    public class DispatchMarketplaceOrderCommandValidator:AbstractValidator<DispatchMarketplaceOrderCommand>
    {
        public DispatchMarketplaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Request.TrackingNumber)
                .MaximumLength(100)
                .When(x => x.Request.TrackingNumber is not null);
        }
    }
}
