using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IDormitoryService
    {
        Task<Dormitory> GetDormitoryByIdAsync(int dormitoryId);
        Task<List<Dormitory>> SearchDormitoriesAsync(string keywords, string city, string nearbyUniversity, string genderType);
        Task<List<Dormitory>> GetDormsByStatusAsync(DormitoryStatus status);
        Task<Dormitory> GetDormitoryDetailsAsync(int dormitoryId);
    }
}
