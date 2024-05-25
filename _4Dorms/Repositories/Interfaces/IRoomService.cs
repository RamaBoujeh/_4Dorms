using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IRoomService
    {
        Task AddRoomAsync(RoomDTO roomDto);
        Task<Room> GetRoomDetailsByDormitoryIdAsync(int dormitoryId);
        Task<bool> DecrementRoomAvailabilityAsync(int dormitoryId, bool isPrivate);
    }
}
