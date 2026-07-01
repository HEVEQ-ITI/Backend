using FluentValidation;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.ConfirmMarketplaceOrderPayment
{
    public sealed class ConfirmMarketplaceOrderPaymentCommandValidator : AbstractValidator<ConfirmMarketplaceOrderPaymentCommand>
    {
        public ConfirmMarketplaceOrderPaymentCommandValidator()
        {
            RuleFor(x => x.MarketplaceOrderId)
                .NotEmpty()
                .WithMessage("MarketplaceOrderId is required.");
        }
    }
}