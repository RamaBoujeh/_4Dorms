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

        public async Task<bool> ProcessPaymentAsync(int paymentGateId, int payerAccount, decimal amount)
        {
            try
            {
                var paymentGate = await _paymentGateRepository.GetByIdAsync(paymentGateId);
                if (paymentGate == null)
                {
                    throw new InvalidOperationException("Payment gate not found.");
                }

                if (paymentGate.PayerAccount != payerAccount)
                {
                    throw new InvalidOperationException("Invalid payer account.");
                }

                paymentGate.PaymentDate = DateTime.UtcNow;
                paymentGate.IsSuccessful = true;

                paymentGate.Amount = amount;

                _paymentGateRepository.Update(paymentGate);

                await _paymentGateRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to process payment.", ex);
            }
        }


    }
}
