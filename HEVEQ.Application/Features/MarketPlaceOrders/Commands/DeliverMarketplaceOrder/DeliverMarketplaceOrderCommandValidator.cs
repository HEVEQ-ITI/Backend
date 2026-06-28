using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.DeliverMarketplaceOrder
{
    public class DeliverMarketplaceOrderCommandValidator:AbstractValidator<DeliverMarketplaceOrderCommand>
    {
        public DeliverMarketplaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
        }
        
    }
}
