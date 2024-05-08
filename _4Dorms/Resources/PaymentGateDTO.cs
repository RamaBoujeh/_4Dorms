using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class PaymentGateDTO
    {
        [CreditCard]
        public int PayerAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string PaymentMethod { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
