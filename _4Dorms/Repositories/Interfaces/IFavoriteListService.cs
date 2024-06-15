using _4Dorms.Models;

namespace _4Dorms.Repositories.Interfaces
{
    public interface IFavoriteListService
    {
        Task<bool> AddDormitoryToFavoritesAsync(int favoriteListId, int dormitoryId);
        Task<bool> RemoveDormitoryFromFavoritesAsync(int favoriteListId, int dormitoryId);
        Task<int?> GetFavoriteDormsAsync(int userId, string userType);
        Task<FavoriteList> GetFavoriteListByIdAsync(int favoriteListId);
    }
}
