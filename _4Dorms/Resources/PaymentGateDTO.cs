using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class PaymentGateDTO
    {
        [Required]
        [CustomCardNumber]  // Apply custom validation attribute
        public string CardNumber { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Range(100, 999, ErrorMessage = "CVV must be a 3-digit number.")]
        public int CVV { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
