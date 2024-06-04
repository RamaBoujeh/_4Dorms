using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace _4Dorms.Repositories.implementation
{
    public class DormitoryService : IDormitoryService
    {
        private readonly IGenericRepository<Dormitory> _genericRepositoryDorm;
        private readonly IGenericRepository<Student> _genericRepositoryStudent;
        private readonly IGenericRepository<DormitoryOwner> _genericRepositoryOwner;
        private readonly ILogger<DormitoryService> _logger;

        public DormitoryService(IGenericRepository<Dormitory> genericRepositoryDorm, IGenericRepository<Student> genericRepositoryStudent,
            IGenericRepository<DormitoryOwner> genericRepositoryOwner, ILogger<DormitoryService> logger)
        {
            _genericRepositoryDorm = genericRepositoryDorm;
            _genericRepositoryStudent = genericRepositoryStudent;
            _genericRepositoryOwner = genericRepositoryOwner;
            _logger = logger;

        }

        public async Task<Dormitory> GetDormitoryByIdAsync(int dormitoryId)
        {
            return await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
        }

        public async Task<List<Dormitory>> SearchDormitoriesAsync(string keywords, string city, string nearbyUniversity, string genderType)
        {
            // Convert search parameters to lowercase for case-insensitive search
            keywords = keywords?.ToLower();
            city = city?.ToLower();
            nearbyUniversity = nearbyUniversity?.ToLower();
            genderType = genderType?.ToLower();

            var query = _genericRepositoryDorm.Query().AsQueryable();

            if (!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(d =>
                    d.DormitoryName.ToLower().Contains(keywords) ||
                    d.City.ToLower().Contains(keywords) ||
                    d.DormitoryDescription.ToLower().Contains(keywords) ||
                    d.NearbyUniversity.ToLower().Contains(keywords) ||
                    d.GenderType.ToLower().Contains(keywords));
            }

            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(d => d.City.ToLower() == city);
            }

            if (!string.IsNullOrEmpty(nearbyUniversity))
            {
                query = query.Where(d => d.NearbyUniversity.ToLower() == nearbyUniversity);
            }

            if (!string.IsNullOrEmpty(genderType))
            {
                query = query.Where(d => d.GenderType.ToLower() == genderType);
            }

            return await query.Include(d => d.ImageUrls).ToListAsync();
        }


        public async Task<List<Dormitory>> GetDormsByStatusAsync(DormitoryStatus status)
        {
            _logger.LogInformation("Fetching dormitories with status: {Status}", status);
            try
            {
                return await _genericRepositoryDorm.Query()
                    .Where(d => d.Status == status)
                    .Include(d => d.ImageUrls)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while querying dormitories by status.");
                throw;
            }
        }

        public async Task<Dormitory> GetDormitoryDetailsAsync(int dormitoryId)
        {
            var dorm = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            return dorm;
        }


    }
}
