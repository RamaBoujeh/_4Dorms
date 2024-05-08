using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System.Data.Entity;
using System.Linq.Expressions;

namespace _4Dorms.Repositories.implementation
{
    public class DormitoryOwnerService : IDormitoryOwnerService
    {
        private readonly IGenericRepository<DormitoryOwner> _genericRepositoryDormitoryOwner;
        private readonly IGenericRepository<Dormitory> _genericRepositoryDorm;

        public DormitoryOwnerService(IGenericRepository<DormitoryOwner> genericRepositoryDormitoryOwner, IGenericRepository<Dormitory> genericRepositoryDorm)
        {
            _genericRepositoryDormitoryOwner = genericRepositoryDormitoryOwner;
            _genericRepositoryDorm = genericRepositoryDorm;

        }

        public async Task SubmitDormitoryForApprovalAsync(DormitoryDTO dormitoryDTO, int dormitoryOwnerId)
        {
            var dormitory = new Dormitory
            {
                DormitoryOwnerId = dormitoryOwnerId,
                DormitoryName = dormitoryDTO.DormitoryName,
                Location = dormitoryDTO.Location,
                Price = dormitoryDTO.Price,
                Amenities = dormitoryDTO.Amenities,
                Status = DormitoryStatus.Pending
            };

            _genericRepositoryDorm.Add(dormitory);
            await _genericRepositoryDorm.SaveChangesAsync();
        }

        public async Task UpdateDormitoryAsync(int dormitoryId, DormitoryDTO updatedDormitoryDTO)
        {
            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                throw new Exception("Dormitory not found");
            }

            dormitory.DormitoryName = updatedDormitoryDTO.DormitoryName;
            dormitory.Location = updatedDormitoryDTO.Location;
            dormitory.Price = updatedDormitoryDTO.Price;
            dormitory.Amenities = updatedDormitoryDTO.Amenities;

            _genericRepositoryDorm.Update(dormitory);
            await _genericRepositoryDorm.SaveChangesAsync();
        }

        public async Task DeleteDormitoryAsync(int dormitoryId)
        {
            var dormitory = await _genericRepositoryDorm.GetByIdAsync(dormitoryId);
            if (dormitory == null)
            {
                throw new Exception("Dormitory not found");
            }

            _genericRepositoryDorm.Remove(dormitoryId);
            await _genericRepositoryDorm.SaveChangesAsync();
        }

    }
}
