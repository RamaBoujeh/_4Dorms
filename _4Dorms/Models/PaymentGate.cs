using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class PaymentGate
    {
        [Key]
        public int PaymentGateId { get; set; }
        [CreditCard]
        public string CardNumber { get; set; }  // Changed to string to handle credit card numbers correctly
        public DateTime PaymentDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CVV { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public PaymentGate()
        {
            Bookings = new HashSet<Booking>();
        }
    }
}
