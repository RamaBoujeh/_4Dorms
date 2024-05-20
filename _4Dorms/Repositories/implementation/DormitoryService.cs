using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

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

        public async Task<List<Dormitory>> SearchDormitoriesAsync(string keywords, string city, string nearbyUniversity, string genderType)
        {
            var query = _genericRepositoryDorm.Query();

            if (!string.IsNullOrEmpty(keywords))
                query = query.Where(d => d.DormitoryDescription.Contains(keywords));

            if (!string.IsNullOrEmpty(city))
                query = query.Where(d => d.City == city);

            if (!string.IsNullOrEmpty(nearbyUniversity))
                query = query.Where(d => d.NearbyUniversity == nearbyUniversity);

            if (!string.IsNullOrEmpty(genderType))
                query = query.Where(d => d.GenderType == genderType);

            return await query.ToListAsync();
        }

        public async Task<List<Dormitory>> GetDormsByStatusAsync(DormitoryStatus status)
        {
            return await _genericRepositoryDorm.GetListByConditionAsync(d => d.Status == status);
                
        }

    }
}
