using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class PaymentGate
    {
        [Key]
        public int PaymentGateId { get; set; }
        [CreditCard]
        public int PayerAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsSuccessful { get; set; }
        public string PaymentMethod { get; set; }
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public PaymentGate()
        {
            Bookings = new HashSet<Booking>();
        }
    }
}
