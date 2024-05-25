using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class BookingRequest
    {
        [Required]
        public BookingDTO BookingDTO { get; set; }
    }
}
