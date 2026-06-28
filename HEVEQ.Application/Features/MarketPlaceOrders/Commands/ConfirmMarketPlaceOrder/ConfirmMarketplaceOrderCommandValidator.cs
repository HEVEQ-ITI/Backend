using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketPlaceOrder
{
    public class ConfirmMarketplaceOrderCommandValidator:AbstractValidator<ConfirmMarketplaceOrderCommand>
    {
        public ConfirmMarketplaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
    }
}
