using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace _4Dorms.Repositories.Implementation
{
    public class FavoriteListService : IFavoriteListService
    {
        private readonly IGenericRepository<FavoriteList> _favoriteListRepository;
        private readonly IGenericRepository<Dormitory> _dormitoryRepository;
        private readonly ILogger<FavoriteListService> _logger;

        public FavoriteListService(
            IGenericRepository<FavoriteList> favoriteListRepository,
            IGenericRepository<Dormitory> dormitoryRepository,
            ILogger<FavoriteListService> logger)
        {
            _favoriteListRepository = favoriteListRepository;
            _dormitoryRepository = dormitoryRepository;
            _logger = logger;
        }

        public async Task<bool> AddDormitoryToFavoritesAsync(int favoriteListId, int dormitoryId)
        {
            try
            {
                _logger.LogInformation("Attempting to add dormitory with ID {DormitoryId} to favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);

                var favoriteList = await _favoriteListRepository.GetByIdAsync(favoriteListId);
                if (favoriteList == null)
                {
                    _logger.LogWarning("Favorite list with ID {FavoriteListId} not found", favoriteListId);
                    return false;
                }

                var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
                if (dormitory == null)
                {
                    _logger.LogWarning("Dormitory with ID {DormitoryId} not found", dormitoryId);
                    return false;
                }

                if (favoriteList.Dormitories.Contains(dormitory))
                {
                    _logger.LogInformation("Dormitory with ID {DormitoryId} is already in the favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);
                    return true; // Already in the list, return true
                }

                favoriteList.Dormitories.Add(dormitory);
                await _favoriteListRepository.SaveChangesAsync();

                _logger.LogInformation("Successfully added dormitory with ID {DormitoryId} to favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding dormitory with ID {DormitoryId} to favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);
                return false;
            }
        }

        public async Task<int?> GetFavoriteDormsAsync(int userId, string userType)
        {
            try
            {
                _logger.LogInformation("Attempting to get favorite list ID for user with ID {UserId} and type {UserType}", userId, userType);

                switch (userType)
                {
                    case "Student":
                        var studentFavoriteList = await _favoriteListRepository.Query()
                            .Include(f => f.Dormitories) // Include dormitories
                            .ThenInclude(d => d.ImageUrls) // Include images
                            .FirstOrDefaultAsync(f => f.StudentId == userId);
                        if (studentFavoriteList == null)
                        {
                            _logger.LogWarning("No favorite list found for student with ID {UserId}", userId);
                            return null;
                        }
                        _logger.LogInformation("Favorite list ID for student with ID {UserId}: {FavoriteListId}", userId, studentFavoriteList.FavoriteId);
                        return studentFavoriteList.FavoriteId;

                    case "DormitoryOwner":
                        var ownerFavoriteList = await _favoriteListRepository.Query()
                            .Include(f => f.Dormitories) // Include dormitories
                            .ThenInclude(d => d.ImageUrls) // Include images
                            .FirstOrDefaultAsync(f => f.DormitoryOwnerId == userId);
                        if (ownerFavoriteList == null)
                        {
                            _logger.LogWarning("No favorite list found for dormitory owner with ID {UserId}", userId);
                            return null;
                        }
                        _logger.LogInformation("Favorite list ID for dormitory owner with ID {UserId}: {FavoriteListId}", userId, ownerFavoriteList.FavoriteId);
                        return ownerFavoriteList.FavoriteId;

                    default:
                        _logger.LogWarning("Invalid user type {UserType}", userType);
                        return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting favorite list ID for user with ID {UserId} and type {UserType}", userId, userType);
                return null;
            }
        }


        public async Task<FavoriteList> GetFavoriteListByIdAsync(int favoriteListId)
        {
            return await _favoriteListRepository.Query()
                .Include(f => f.Dormitories) // Eagerly load dormitories
                .FirstOrDefaultAsync(f => f.FavoriteId == favoriteListId);
        }

        public async Task<bool> RemoveDormitoryFromFavoritesAsync(int favoriteListId, int dormitoryId)
        {
            try
            {
                _logger.LogInformation("Attempting to remove dormitory with ID {DormitoryId} from favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);

                var favoriteList = await _favoriteListRepository.Query()
                    .Include(f => f.Dormitories)
                    .FirstOrDefaultAsync(f => f.FavoriteId == favoriteListId);

                if (favoriteList == null)
                {
                    _logger.LogWarning("Favorite list with ID {FavoriteListId} not found", favoriteListId);
                    return false;
                }

                var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
                if (dormitory == null)
                {
                    _logger.LogWarning("Dormitory with ID {DormitoryId} not found", dormitoryId);
                    return false;
                }

                if (!favoriteList.Dormitories.Any(d => d.DormitoryId == dormitoryId))
                {
                    _logger.LogInformation("Dormitory with ID {DormitoryId} is not in the favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);
                    return true; // Already removed, return true
                }

                favoriteList.Dormitories.Remove(dormitory);
                await _favoriteListRepository.SaveChangesAsync();

                _logger.LogInformation("Successfully removed dormitory with ID {DormitoryId} from favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing dormitory with ID {DormitoryId} from favorite list with ID {FavoriteListId}", dormitoryId, favoriteListId);
                return false;
            }
        }
    }
}
