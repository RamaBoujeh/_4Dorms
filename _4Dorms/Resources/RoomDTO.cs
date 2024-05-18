using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class RoomDTO
    {
        public bool? privateRoom { get; set; }
        public bool? SharedRoom { get; set; }
        public int? NumOfprivateRooms { get; set; }
        public int? NumOfSharedRooms { get; set; }
    }
}
