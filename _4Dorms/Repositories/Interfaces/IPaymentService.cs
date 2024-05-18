using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(PaymentGateDTO paymentDto);
    }
}
