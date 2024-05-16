namespace _4Dorms.Repositories.Interfaces
{
    public interface IFavoriteListService
    {
        Task<bool> AddDormitoryToFavoritesAsync(int favoriteListId, int dormitoryId);
        Task<bool> RemoveDormitoryFromFavoritesAsync(int favoriteListId, int dormitoryId);
    }
}
