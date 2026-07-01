using FluentValidation;

namespace HEVEQ.Application.Features.MarketPlaceOrders.Commands.CheckoutMarketplaceOrderPayment
{
    public sealed class CheckoutMarketplaceOrderPaymentCommandValidator : AbstractValidator<CheckoutMarketplaceOrderPaymentCommand>
    {
        public CheckoutMarketplaceOrderPaymentCommandValidator()
        {
            RuleFor(x => x.MarketplaceOrderId)
                .NotEmpty()
                .WithMessage("MarketplaceOrderId is required.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .WithMessage("Payment method is required.")
                .MaximumLength(50)
                .WithMessage("Payment method cannot exceed 50 characters.");

            RuleFor(x => x.SuccessUrl)
                .MaximumLength(500)
                .WithMessage("SuccessUrl cannot exceed 500 characters.");

            RuleFor(x => x.CancelUrl)
                .MaximumLength(500)
                .WithMessage("CancelUrl cannot exceed 500 characters.");
        }
    }
}