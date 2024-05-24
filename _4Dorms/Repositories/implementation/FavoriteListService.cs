using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace _4Dorms.Repositories.Implementation
{
    public class FavoriteListService : IFavoriteListService
    {
        private readonly IGenericRepository<FavoriteList> _favoriteListRepository;
        private readonly IGenericRepository<Dormitory> _dormitoryRepository;
        private readonly ILogger<FavoriteListService> _logger;

        public FavoriteListService(IGenericRepository<FavoriteList> favoriteListRepository, IGenericRepository<Dormitory> dormitoryRepository, ILogger<FavoriteListService> logger)
        {
            _favoriteListRepository = favoriteListRepository;
            _dormitoryRepository = dormitoryRepository;
            _logger = logger;
        }

        public async Task<bool> AddDormitoryToFavoritesAsync(int favoriteListId, int dormitoryId)
        {
            try
            {
                _logger.LogInformation("Fetching favorite list with ID: {FavoriteListId}", favoriteListId);
                var favoriteList = await _favoriteListRepository.GetByIdAsync(favoriteListId);
                if (favoriteList == null)
                {
                    _logger.LogWarning("Favorite list not found for ID: {FavoriteListId}", favoriteListId);
                    return false;
                }

                _logger.LogInformation("Fetching dormitory with ID: {DormitoryId}", dormitoryId);
                var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
                if (dormitory == null)
                {
                    _logger.LogWarning("Dormitory not found for ID: {DormitoryId}", dormitoryId);
                    return false;
                }

                favoriteList.Dormitories.Add(dormitory);
                await _favoriteListRepository.SaveChangesAsync();

                _logger.LogInformation("Dormitory with ID: {DormitoryId} added to favorite list with ID: {FavoriteListId}", dormitoryId, favoriteListId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding dormitory to favorites.");
                return false;
            }
        }

        public async Task<bool> RemoveDormitoryFromFavoritesAsync(int favoriteListId, int dormitoryId)
        {
            try
            {
                _logger.LogInformation("Fetching favorite list with ID: {FavoriteListId}", favoriteListId);
                var favoriteList = await _favoriteListRepository.GetByIdAsync(favoriteListId);
                if (favoriteList == null)
                {
                    _logger.LogWarning("Favorite list not found for ID: {FavoriteListId}", favoriteListId);
                    return false;
                }

                _logger.LogInformation("Fetching dormitory with ID: {DormitoryId}", dormitoryId);
                var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
                if (dormitory == null)
                {
                    _logger.LogWarning("Dormitory not found for ID: {DormitoryId}", dormitoryId);
                    return false;
                }

                favoriteList.Dormitories.Remove(dormitory);
                await _favoriteListRepository.SaveChangesAsync();

                _logger.LogInformation("Dormitory with ID: {DormitoryId} removed from favorite list with ID: {FavoriteListId}", dormitoryId, favoriteListId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing dormitory from favorites.");
                return false;
            }
        }
    }
}
