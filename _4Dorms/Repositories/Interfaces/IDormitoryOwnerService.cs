using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IDormitoryOwnerService
    {
        Task<IEnumerable<DormitoryOwner>> GetAllOwnersAsync();
        Task<DormitoryOwnerDTO> GetOwnerByIdAsync(int ownerId);
        Task SubmitDormitoryForApprovalAsync(DormitorySubmitDTO dormitoryDTO, int dormitoryOwnerId, List<IFormFile> images);
        Task UpdateDormitoryAsync(int dormitoryId, DormitoryDTO updatedDormitoryDTO);
        Task DeleteDormitoryAsync(int dormitoryId);
    }
}