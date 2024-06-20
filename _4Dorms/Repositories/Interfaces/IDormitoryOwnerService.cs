using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IDormitoryOwnerService
    {
        Task<IEnumerable<DormitoryOwner>> GetAllOwnersAsync();
        Task<DormitoryOwnerDTO> GetOwnerByIdAsync(int ownerId);
        Task SubmitDormitoryForApprovalAsync(DormitorySubmitDTO dormitoryDTO, int dormitoryOwnerId, List<IFormFile> images);
        Task DeleteDormitoryAsync(int dormitoryId);
        Task<Result> DeleteImageAsync(int dormitoryId, string fileName);
        Task<Result> UploadImagesAsync(int dormitoryId, List<IFormFile> images);
        Task UpdateDormitoryAsync(int dormitoryId, DormitoryDTO updatedDormitoryDTO, List<IFormFile> newImages);
    }
}