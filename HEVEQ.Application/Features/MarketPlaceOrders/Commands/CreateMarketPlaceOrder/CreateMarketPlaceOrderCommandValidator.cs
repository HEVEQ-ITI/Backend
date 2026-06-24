using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CreateMarketPlaceOrder
{
    public class CreateMarketPlaceOrderCommandValidator:AbstractValidator<CreateMarketPlaceOrderCommand>
    {
        public CreateMarketPlaceOrderCommandValidator()
        {
            RuleFor(x => x.Request.ListingId).NotEmpty();
            RuleFor(x => x.Request.DeliveryAddress).MaximumLength(500);
            RuleFor(x => x.Request.DeliveryPreference).IsInEnum().When(x => x.Request.DeliveryPreference.HasValue);
        }
    }
}
