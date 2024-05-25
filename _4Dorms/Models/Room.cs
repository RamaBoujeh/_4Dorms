using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public bool? PrivateRoom { get; set; }
        public bool? SharedRoom { get; set; }
        public int? NumOfPrivateRooms { get; set; }
        public int? NumOfSharedRooms { get; set; }

        [ForeignKey("DormitoryId")]
        public int? DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public Room()
        {
            Bookings = new HashSet<Booking>();
        }
    }
}
