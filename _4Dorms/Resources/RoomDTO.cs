using System.ComponentModel.DataAnnotations;

namespace _4Dorms.Resources
{
    public class RoomDTO
    {
        public bool? PrivateRoom { get; set; }  // Changed to PascalCase
        public bool? SharedRoom { get; set; }  // Changed to PascalCase
        public int? NumOfPrivateRooms { get; set; }  // Changed to PascalCase
        public int? NumOfSharedRooms { get; set; }  // Changed to PascalCase
        public int? DormitoryId { get; set; }

    }

}
