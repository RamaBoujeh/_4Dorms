using _4Dorms.GenericRepo;
using _4Dorms.Models;
using _4Dorms.Repositories.implementation;
using _4Dorms.Repositories.Interfaces;
using _4Dorms.Resources;
using System.Linq;
using System.Threading.Tasks;

namespace _4Dorms.Repositories.Implementation
{
    public class DormitoryOwnerService : IDormitoryOwnerService
    {
        private readonly IGenericRepository<DormitoryOwner> _genericRepositoryDormitoryOwner;
        private readonly IGenericRepository<Dormitory> _genericRepositoryDorm;
        private readonly IGenericRepository<Room> _genericRepositoryRoom;
        private readonly IGenericRepository<DormitoryImage> _genericRepositoryDormitoryImage;
        private readonly ILogger<DormitoryService> _logger;

        public DormitoryOwnerService(IGenericRepository<DormitoryOwner> genericRepositoryDormitoryOwner, IGenericRepository<Dormitory> genericRepositoryDorm,
            IGenericRepository<Room> genericRepositoryRoom, IGenericRepository<DormitoryImage> genericRepositoryDormitoryImage, ILogger<DormitoryService> logger)
        {
            _genericRepositoryDormitoryOwner = genericRepositoryDormitoryOwner;
            _genericRepositoryDorm = genericRepositoryDorm;
            _genericRepositoryRoom = genericRepositoryRoom;
            _genericRepositoryDormitoryImage = genericRepositoryDormitoryImage;
            _logger = logger;
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
                phone = dormitoryDTO.Phone,
                Email = dormitoryDTO.Email,
                DormitoryDescription = dormitoryDTO.DormitoryDescription,
                PriceFullYear = dormitoryDTO.PriceFullYear,
                PriceHalfYear = dormitoryDTO.PriceHalfYear,
                Location = dormitoryDTO.Location,
                Status = DormitoryStatus.Pending
            };

            await _genericRepositoryDorm.Add(dormitory);
            await _genericRepositoryDorm.SaveChangesAsync();

            _logger.LogInformation("Dormitory saved with ID: {DormitoryId}", dormitory.DormitoryId);

            if (dormitoryDTO.ImageUrls != null && dormitoryDTO.ImageUrls.Any())
            {
                var imageUrls = dormitoryDTO.ImageUrls.Select(url => new DormitoryImage { Url = url, DormitoryId = dormitory.DormitoryId }).ToList();
                foreach (var imageUrl in imageUrls)
                {
                    await _genericRepositoryDormitoryImage.Add(imageUrl);
                    _logger.LogInformation("Saved image with URL: {Url} for Dormitory ID: {DormitoryId}", imageUrl.Url, dormitory.DormitoryId);
                }
                await _genericRepositoryDormitoryImage.SaveChangesAsync();
            }

            if (dormitoryDTO.RoomDTO != null)
            {
                var room = new Room
                {
                    privateRoom = dormitoryDTO.RoomDTO.PrivateRoom,
                    SharedRoom = dormitoryDTO.RoomDTO.SharedRoom,
                    NumOfprivateRooms = dormitoryDTO.RoomDTO.NumOfPrivateRooms,
                    NumOfSharedRooms = dormitoryDTO.RoomDTO.NumOfSharedRooms,
                    DormitoryId = dormitory.DormitoryId
                };

                await _genericRepositoryRoom.Add(room);
                await _genericRepositoryRoom.SaveChangesAsync();
                _logger.LogInformation("Room saved for Dormitory ID: {DormitoryId}", dormitory.DormitoryId);
            }
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
            dormitory.phone = updatedDormitoryDTO.Phone;
            dormitory.Email = updatedDormitoryDTO.Email;
            dormitory.DormitoryDescription = updatedDormitoryDTO.DormitoryDescription;
            dormitory.PriceHalfYear = updatedDormitoryDTO.PriceHalfYear;
            dormitory.PriceFullYear = updatedDormitoryDTO.PriceFullYear;

            // Remove existing images
            var imagesToRemove = dormitory.ImageUrls.ToList();
            foreach (var image in imagesToRemove)
            {
                _genericRepositoryDorm.Remove(image.ImageId);
            }

            // Add new images
            dormitory.ImageUrls.Clear();
            dormitory.ImageUrls = updatedDormitoryDTO.ImageUrls.Select(url => new DormitoryImage { Url = url }).ToList();

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

            // Remove associated images
            var imagesToRemove = dormitory.ImageUrls.ToList();
            foreach (var image in imagesToRemove)
            {
                _genericRepositoryDorm.Remove(image.ImageId);
            }

            // Remove the dormitory
            _genericRepositoryDorm.Remove(dormitoryId);
            await _genericRepositoryDorm.SaveChangesAsync();
        }
    }
}