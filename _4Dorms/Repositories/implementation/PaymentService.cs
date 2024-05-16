using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;

namespace _4Dorms.Repositories.implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly GenericRepository<PaymentGate> _paymentGateRepository;

        public PaymentService(GenericRepository<PaymentGate> paymentGateRepository)
        {
            _paymentGateRepository = paymentGateRepository;
        }

        public Task<bool> ProcessPayment(int cardNumber, DateTime expirationDate, int cvv, decimal amount)
        {
            // Emulated payment processing logic
            if (expirationDate > DateTime.Now && cvv.ToString().Length == 3)
            {
                // Simulate a successful payment process
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

    }
}
