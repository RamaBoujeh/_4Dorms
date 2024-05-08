using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string Amenities { get; set; }
        public bool IsAvailabile { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int RoomNumber { get; set; }
        [ForeignKey("DormitoryId")]
        public string RoomType { get; set; }
        public int? DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public Room()
        {
            Bookings = new HashSet<Booking>();
        }
    }
}
