using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IAdministratorService
    {
        Task ReviewDormitoryAsync(int dormitoryId, bool approved, int administratorId);
        Task<bool> DeleteUserProfileByAdminAsync(int userId, UserType userType);
    }
}
