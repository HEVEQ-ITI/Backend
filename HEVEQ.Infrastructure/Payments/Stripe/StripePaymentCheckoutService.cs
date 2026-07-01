using HEVEQ.Application.Common.Interfaces;
using HEVEQ.Application.Common.Payments;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace HEVEQ.Infrastructure.Payments.Stripe
{
    public class StripePaymentCheckoutService : IPaymentCheckoutService
    {
        private readonly StripePaymentOptions _options;
        public StripePaymentCheckoutService(IOptions<StripePaymentOptions> options)
        {
            _options = options.Value;
        }

        public async Task<PaymentCheckoutResult> CreateCheckoutAsync(PaymentCheckoutCreateRequest request, CancellationToken cancellationToken)
        {
            if (_options.UseSimulatedCheckout)
            {
                var checkoutUrl =
                    $"{_options.SimulatedCheckoutBaseUrl}" +
                    $"?referenceType={request.ReferenceType}" +
                    $"&referenceId={request.ReferenceId}" +
                    $"&amount={request.Amount}" +
                    $"&currency={request.Currency}";

                return new PaymentCheckoutResult
                {
                    PaymentProvider = "Stripe",
                    CheckoutUrl = checkoutUrl,
                    Status = "CheckoutCreated",
                    PaymentGatewayReference = $"sim_{request.ReferenceType}_{request.ReferenceId}"
                };
            }

            if (string.IsNullOrWhiteSpace(_options.SecretKey))
                throw new InvalidOperationException("Stripe SecretKey is missing.");

            var successUrl = string.IsNullOrWhiteSpace(request.SuccessUrl) ? _options.DefaultSuccessUrl : request.SuccessUrl;

            var cancelUrl = string.IsNullOrWhiteSpace(request.CancelUrl) ? _options.DefaultCancelUrl : request.CancelUrl;

            var sessionOptions = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                ClientReferenceId = $"{request.ReferenceType}:{request.ReferenceId}",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Metadata = new Dictionary<string, string>
                {
                    ["referenceType"] = request.ReferenceType.ToString(),
                    ["referenceId"] = request.ReferenceId.ToString(),
                    ["referenceNumber"] = request.ReferenceNumber,
                    ["payingUserId"] = request.PayingUserId.ToString()
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = request.Currency.ToLowerInvariant(),
                            UnitAmount = ToMinorUnits(request.Amount),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"ShareGear {request.ReferenceType} {request.ReferenceNumber}"
                            }
                        }
                    }
                }
            };

            var service = new SessionService();

            var session = await service.CreateAsync(
                sessionOptions,
                new RequestOptions
                {
                    ApiKey = _options.SecretKey
                },
                cancellationToken);

            if (string.IsNullOrWhiteSpace(session.Url))
                throw new InvalidOperationException("Stripe did not return a checkout URL.");

            return new PaymentCheckoutResult
            {
                PaymentProvider = "Stripe",
                CheckoutUrl = session.Url,
                Status = "CheckoutCreated",
                PaymentGatewayReference = session.Id
            };
        }

        private static long ToMinorUnits(decimal amount)
        {
            return (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
        }
    }
}