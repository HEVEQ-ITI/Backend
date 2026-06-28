using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CompleteMarketplaceOrder
{
    public class CompleteMarketplaceOrderCommandValidator:AbstractValidator<CompleteMarketplaceOrderCommand>
    {
        public CompleteMarketplaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }

    }
}
