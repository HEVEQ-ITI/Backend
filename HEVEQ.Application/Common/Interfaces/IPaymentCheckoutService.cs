using HEVEQ.Application.Common.Payments;

namespace HEVEQ.Application.Common.Interfaces
{
    public interface IPaymentCheckoutService
    {
        Task<PaymentCheckoutResult> CreateCheckoutAsync(PaymentCheckoutCreateRequest request, CancellationToken cancellationToken);
    }
}