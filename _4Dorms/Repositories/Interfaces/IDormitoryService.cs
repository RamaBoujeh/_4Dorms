using _4Dorms.Models;
using _4Dorms.Resources;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IDormitoryService
    {
        Task<bool> SelectDormitoryAsync(int dormitoryId);
        Task<List<Dormitory>> SearchDormitoriesAsync(string dormName, string location, decimal? price, bool sortByLowToHigh);
    }
}
