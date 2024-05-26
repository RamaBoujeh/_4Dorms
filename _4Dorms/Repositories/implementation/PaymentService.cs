using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<PaymentGate> _paymentGateRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public PaymentService(IGenericRepository<PaymentGate> paymentGateRepository, IGenericRepository<Booking> bookingRepository)
        {
            _paymentGateRepository = paymentGateRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentGateDTO paymentDto)
        {
            if (IsValidPaymentData(paymentDto))
            {
                var payment = new PaymentGate
                {
                    CardNumber = paymentDto.CardNumber,
                    ExpirationDate = paymentDto.ExpirationDate,
                    CVV = paymentDto.CVV,
                    Amount = paymentDto.Amount,
                    PaymentDate = DateTime.Now
                };

                _paymentGateRepository.Add(payment);
                return await _paymentGateRepository.SaveChangesAsync();
            }
            return false;
        }

        private bool IsValidPaymentData(PaymentGateDTO paymentDto)
        {
            if (paymentDto.ExpirationDate > DateTime.Now && paymentDto.CVV.ToString().Length == 3)
            {
                return true;
            }
            return false;
        }
    }
}
