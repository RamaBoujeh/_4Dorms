using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.implementation
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Room> _roomRepository;

        public RoomService(IGenericRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddRoomAsync(bool? isPrivate, bool? isShared, int? numOfPrivateRooms, int? numOfSharedRooms, int? dormitoryId)
        {
            var room = new Room
            {
                privateRoom = isPrivate,
                SharedRoom = isShared,
                NumOfprivateRooms = numOfPrivateRooms,
                NumOfSharedRooms = numOfSharedRooms,
                DormitoryId = dormitoryId
            };
        }
    }
}
