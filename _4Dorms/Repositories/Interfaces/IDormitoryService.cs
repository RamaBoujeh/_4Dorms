using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IDormitoryService
    {
        Task<bool> SelectDormitoryAsync(int dormitoryId);
        Task<List<Dormitory>> SearchDormitoriesAsync(string keywords, string city, string nearbyUniversity, string genderType);
        Task<List<Dormitory>> GetDormsByStatusAsync(DormitoryStatus status);
    }
}
