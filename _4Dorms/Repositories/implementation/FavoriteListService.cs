using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;

namespace _4Dorms.Repositories.implementation
{
    public class FavoriteListService : IFavoriteListService
    {
        private readonly IGenericRepository<FavoriteList> _favoriteListRepository;
        private readonly IGenericRepository<Dormitory> _dormitoryRepository;

        public FavoriteListService(IGenericRepository<FavoriteList> favoriteListRepository, IGenericRepository<Dormitory> dormitoryRepository)
        {
            _favoriteListRepository = favoriteListRepository;
            _dormitoryRepository = dormitoryRepository;
        }
        public async Task<bool> AddDormitoryToFavoritesAsync(int favoriteListId, int dormitoryId)
        {
            var favoriteList = await _favoriteListRepository.GetByIdAsync(favoriteListId);
            if (favoriteList == null)
            {
                return false;
            }
            var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                return false;
            }

            favoriteList.Dormitories.Add(dormitory);

            await _favoriteListRepository.SaveChangesAsync();

            return true;
        }
    }
}


