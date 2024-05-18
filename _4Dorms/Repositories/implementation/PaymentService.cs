using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly GenericRepository<PaymentGate> _paymentGateRepository;

        public PaymentService(GenericRepository<PaymentGate> paymentGateRepository)
        {
            _paymentGateRepository = paymentGateRepository;
        }

        public Task<bool> ProcessPaymentAsync(PaymentGateDTO paymentDto)
        {
            // Emulated payment processing logic
            if (IsValidPaymentData(paymentDto))
            {
                // Simulate a successful payment process
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private bool IsValidPaymentData(PaymentGateDTO paymentDto)
        {
            // Add validation logic here
            if (paymentDto.ExpirationDate > DateTime.Now && paymentDto.CVV.ToString().Length == 3)
            {
                // Assume valid if expiration date is in the future and CVV is 3 digits
                return true;
            }
            return false;
        }

    }
}
