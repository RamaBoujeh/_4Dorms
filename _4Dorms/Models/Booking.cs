using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _4Dorms.Models
{
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }

    public enum Duration
    {
        SixMonths = 0,
        TwelveMonths = 1
    }


    public enum RoomType
    {
        Private = 0,
        Shared = 1
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
        public Duration Duration { get; set; }
        public RoomType RoomType { get; set; }
        public int? DormitoryOwnerId { get; set; }
        public DormitoryOwner DormitoryOwner { get; set; }

        [ForeignKey("PaymentId")]
        public int? PaymentGateId { get; set; }
        public PaymentGate PaymentGate { get; set; }

        public Room Room { get; set; }
    }
}
