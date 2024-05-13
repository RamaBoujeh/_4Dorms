using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IRoomService
    {
        Task AddRoomAsync(bool? isPrivate, bool? isShared, int? numOfPrivateRooms, int? numOfSharedRooms, int? dormitoryId);
    }
}
