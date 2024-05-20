using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Models
{
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [ForeignKey("RoomId")]
        public int? RoomId { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("DormitoryId")]
        public int? DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public Status Status { get; set; }
        [ForeignKey("DormitoryOwnerId")]
        public int? DormitoryOwnerId { get; set; }
        public DormitoryOwner DormitoryOwner { get; set; }

        [ForeignKey("PaymentId")]
        public int? PaymentGateId { get; set; }
        public PaymentGate PaymentGate { get; set; }

        public Room Room { get; set; }
    }
}
