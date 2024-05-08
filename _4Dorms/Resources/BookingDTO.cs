using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class BookingDTO
    {
        public int RoomId { get; set; }
        public int DormitoryId { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

    }
}
