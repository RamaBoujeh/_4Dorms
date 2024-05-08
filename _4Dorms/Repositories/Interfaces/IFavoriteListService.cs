namespace _4Dorms.Repositories.Interfaces
{
    public interface IFavoriteListService
    {
        Task<bool> AddDormitoryToFavoritesAsync(int favoriteListId, int dormitoryId);
    }
}
