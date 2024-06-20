using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class RoomDTO
    {
        public bool? PrivateRoom { get; set; }
        public bool? SharedRoom { get; set; }
        public int? NumOfPrivateRooms { get; set; }
        public int? NumOfSharedRooms { get; set; }
        public int DormitoryId { get; set; }

    }

}
