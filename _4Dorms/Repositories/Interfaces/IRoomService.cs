using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IRoomService
    {
        Task<bool> AddRoomAsync(RoomDTO roomDTO);
    }
}
