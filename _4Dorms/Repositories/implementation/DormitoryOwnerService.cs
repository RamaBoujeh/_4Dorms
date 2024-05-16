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
                GenderType = dormitoryDTO.GenderType,
                City = dormitoryDTO.City,
                NearbyUniversity = dormitoryDTO.NearbyUniversity,
                phone = dormitoryDTO.phone,
                Email = dormitoryDTO.Email,
                DormitoryDescription = dormitoryDTO.DormitoryDescription,
                PriceFullYear = dormitoryDTO.PriceFullYear,
                PriceHalfYear = dormitoryDTO.PriceHalfYear,
                Location = dormitoryDTO.Location,
                Status = DormitoryStatus.Pending
            };

            foreach (var imageUrl in dormitoryDTO.ImageUrls)
            {
                dormitory.ImageUrls.Add(new DormitoryImage { Url = imageUrl });
            }

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
            dormitory.GenderType = updatedDormitoryDTO.GenderType;
            dormitory.City = updatedDormitoryDTO.City;
            dormitory.NearbyUniversity = updatedDormitoryDTO.NearbyUniversity;
            dormitory.phone = updatedDormitoryDTO.phone;
            dormitory.Email = updatedDormitoryDTO.Email;
            dormitory.DormitoryDescription = updatedDormitoryDTO.DormitoryDescription;
            dormitory.PriceHalfYear = updatedDormitoryDTO.PriceHalfYear;
            dormitory.PriceFullYear = updatedDormitoryDTO.PriceFullYear;
            foreach (var image in dormitory.ImageUrls.ToList())
            {
                _genericRepositoryDorm.Remove(image.ImageId);
            }
            dormitory.ImageUrls.Clear();

            foreach (var imageUrl in updatedDormitoryDTO.ImageUrls)
            {
                dormitory.ImageUrls.Add(new DormitoryImage { Url = imageUrl });
            }

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

            foreach (var image in dormitory.ImageUrls.ToList())
            {
                _genericRepositoryDorm.Remove(image.ImageId);
            }

            _genericRepositoryDorm.Remove(dormitoryId);
            await _genericRepositoryDorm.SaveChangesAsync();
        }

    }
}
