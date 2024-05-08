using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _4Dorms.Repositories.implementation
{
    public class DormitoryService : IDormitoryService
    {
        private readonly IGenericRepository<Dormitory> _genericRepositoryDorm;
        private readonly IGenericRepository<Student> _genericRepositoryStudent;
        private readonly IGenericRepository<DormitoryOwner> _genericRepositoryOwner;

        public DormitoryService(IGenericRepository<Dormitory> genericRepositoryDorm, IGenericRepository<Student> genericRepositoryStudent, IGenericRepository<DormitoryOwner> genericRepositoryOwner)
        {
            _genericRepositoryDorm = genericRepositoryDorm;
            _genericRepositoryStudent = genericRepositoryStudent;
            _genericRepositoryOwner = genericRepositoryOwner;
        }

        public async Task<bool> SelectDormitoryAsync(int dormitoryId)
        {
            try
            {
                var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
                if (dormitory == null)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error selecting dormitory: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Dormitory>> SearchDormitoriesAsync(string dormName, string location, decimal? price, bool sortByLowToHigh)
        {
            var query = _genericRepositoryDorm.Query();

            if (!string.IsNullOrEmpty(dormName))
                query = query.Where(d => d.DormitoryName.Equals(dormName));

            if (!string.IsNullOrEmpty(location))
                query = query.Where(d => d.Location.Equals(location));

            if (price.HasValue)
                query = query.Where(d => d.Price == price.Value);

            if (sortByLowToHigh)
                query = query.OrderBy(d => d.Price);
            else
                query = query.OrderByDescending(d => d.Price);

            return await query.ToListAsync();
        }

    }
}
