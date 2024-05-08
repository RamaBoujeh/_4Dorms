using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IDormitoryOwnerService
    {
        Task SubmitDormitoryForApprovalAsync(DormitoryDTO dormitoryDTO, int dormitoryOwnerId);
        Task UpdateDormitoryAsync(int dormitoryId, DormitoryDTO updatedDormitoryDTO);
        Task DeleteDormitoryAsync(int dormitoryId);
    }
}
