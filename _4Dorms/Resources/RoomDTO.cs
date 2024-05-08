using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class RoomDTO
    {
        public string Amenities { get; set; }
        public decimal Price { get; set; }
        public int RoomNumber { get; set; }
        public int DormitoryId { get; set; }
    }
}
