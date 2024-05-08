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

        public async Task<bool> AddRoomAsync(RoomDTO roomDTO)
        {
            var room = new Room
            {
                Amenities = roomDTO.Amenities,
                Price = roomDTO.Price,
                RoomNumber = roomDTO.RoomNumber,
                DormitoryId = roomDTO.DormitoryId
            };

            _roomRepository.Add(room);
            return await _roomRepository.SaveChangesAsync();
        }
    }
}
