using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.implementation
{
    public class RoomService : IRoomService
    {
        private readonly IGenericRepository<Room> _roomRepository;

        public RoomService(IGenericRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task AddRoomAsync(RoomDTO roomDto)
        {
            var room = new Room
            {
                PrivateRoom = roomDto.PrivateRoom,
                SharedRoom = roomDto.SharedRoom,
                NumOfPrivateRooms = roomDto.NumOfPrivateRooms,
                NumOfSharedRooms = roomDto.NumOfSharedRooms,
                DormitoryId = roomDto.DormitoryId
            };

            _roomRepository.Add(room);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task<Room> GetRoomDetailsByDormitoryIdAsync(int dormitoryId)
        {
            return await _roomRepository.FindByConditionAsync(r => r.DormitoryId == dormitoryId);
        }

        public async Task<bool> DecrementRoomAvailabilityAsync(int dormitoryId, bool isPrivate)
        {
            var room = await _roomRepository.FindByConditionAsync(r => r.DormitoryId == dormitoryId);
            if (room == null)
            {
                return false;
            }

            if (isPrivate && room.NumOfPrivateRooms > 0)
            {
                room.NumOfPrivateRooms--;
            }
            else if (!isPrivate && room.NumOfSharedRooms > 0)
            {
                room.NumOfSharedRooms--;
            }
            else
            {
                return false;
            }

            _roomRepository.Update(room);
            return await _roomRepository.SaveChangesAsync();
        }
    }
}
