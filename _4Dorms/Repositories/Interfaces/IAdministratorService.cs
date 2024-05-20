using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IAdministratorService
    {
        Task ReviewDormitoryAsync(int dormitoryId, bool approved, int administratorId);
        Task<bool> DeleteUserProfileAsync(int userId, UserType userType);
        Task<bool> UpdateDormStatusAsync(int dormId, DormitoryStatus status);
    }
}
