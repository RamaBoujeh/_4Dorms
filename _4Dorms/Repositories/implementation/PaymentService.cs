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

        

    }
}
